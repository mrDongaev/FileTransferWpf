using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTransferWpfApp.Model.ModelSettings;
using FileTransferWpfApp.Model.ModelLogs;

namespace FileTransferWpfApp.Model.ModelHandlers
{
    public class FileWatcher : IDisposable
    {
        private bool _disposed;
        public DirectorySettings? DirectorySettings { get; private set; }
        public FileSystemWatcher? SystemWatcher { get; private set; }
        public bool Enabled { get; set; }
        public Dictionary<string, DateTime> ProcessedFiles { get; private set; } = [];
        public Dictionary<string, DateTime> CorruptedFiles { get; private set; } = [];
        private int RestartsCount { get; set; } = 0;

        private Task _timerTask;

        public FileWatcher(DirectorySettings settings)
        {
            try
            {
                DirectorySettings = settings ?? throw new ArgumentNullException(nameof(settings));

                if (!Directory.Exists(DirectorySettings.MoveFromPath))
                {
                    throw new DirectoryNotFoundException(DirectorySettings.MoveFromPath);
                }
                else
                {
                    PassiveFileWatcher();
                    //Если таймаут не настроен - периодически раз в час передергивать FileWatcher, тк исторически было зафиксировано его странное поведение
                    //_timer = new Timer(new TimerCallback(passiveFileWatcher), DirectorySettings, 1, 3600000);
                }
                DataWarehouseModel.AddUILog(new UILogModel((
                    $@"Запущен Watcher файлов [{DirectorySettings.FileFilterMask}] в папке [{DirectorySettings.MoveFromPath}]
    Перемещение в папки:
        {string.Join("\r\n\t", DirectorySettings.MoveToPaths)}"), DataWarehouseModel.ImportanceLogs.low));
            }
            catch (Exception ex)
            {
                DataWarehouseModel.AddUILog(new UILogModel(ex.ToString(), DataWarehouseModel.ImportanceLogs.low));

                RestartWatcher();
            }
        }
        /// <summary>
        /// Включить наблюдатель файловой системы
        /// </summary>
        private void PassiveFileWatcher()
        {
            DataWarehouseModel.AddUILog(new UILogModel($"Запуск Watcher в автоматическом режиме"
    , DataWarehouseModel.ImportanceLogs.low));

            LoggerModel.Info("Запуск Watcher в автоматическом режиме");

            try
            {
                if (SystemWatcher != null) 
                {
                    SystemWatcher.Dispose();
                }

                if (!Directory.Exists(DirectorySettings.MoveFromPath)) 
                {
                    return;
                } 

                SystemWatcher = new FileSystemWatcher
                {
                    Path = DirectorySettings.MoveFromPath,

                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,

                    Filter = DirectorySettings.FileFilterMask
                };
                SystemWatcher.Changed += new FileSystemEventHandler(OnChanged);


                SystemWatcher.Error += new ErrorEventHandler(SystemWatcher_Error);

                SystemWatcher.EnableRaisingEvents = true;

                Enabled = true;
               
            }
            catch (Exception ex)
            {
                LoggerModel.Error(ex);
                RestartWatcher();
            }
        }

        private void SystemWatcher_Error(object sender, ErrorEventArgs e)
        {
            DataWarehouseModel.AddUILog(new UILogModel(e.GetException() + $"\r\n[{DirectorySettings.MoveFromPath}]", DataWarehouseModel.ImportanceLogs.low));

            LoggerModel.Error(e.GetException() + $"\r\n[{DirectorySettings.MoveFromPath}]");

            //if (Enabled) return;
            RestartWatcher();
        }

        private void RestartWatcher()
        {
            Enabled = false;

            while (!Enabled)
            {
                DataWarehouseModel.AddUILog(new UILogModel($"Возникла проблема. Производится переподключение к папке [{DirectorySettings.MoveFromPath}]"
                    , DataWarehouseModel.ImportanceLogs.low));

                LoggerModel.Warn($"Возникла проблема. Производится переподключение к папке [{DirectorySettings.MoveFromPath}]");

                Thread.Sleep(1000);

                if (SystemWatcher != null) 
                {
                    SystemWatcher.Dispose();
                }
                    
                RestartsCount++;

                //PassiveFileWatcher();
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Transfer(e.FullPath);
        }

        private void Transfer(string filePath)
        {
            int numTries = 0;

            if (!File.Exists(filePath)) 
            {
                return;
            }
            DataWarehouseModel.AddUILog(new UILogModel($"Start transferring file: [{filePath}]",
    DataWarehouseModel.ImportanceLogs.low));

            LoggerModel.Info($"Start transferring file: [{filePath}]");

            DateTime lastWriteDate = File.GetLastWriteTimeUtc(filePath);

            if (CorruptedFiles.Contains(new KeyValuePair<string, DateTime>(filePath, lastWriteDate))) 
            {
                return;
            }
            while (true)
            {
                ++numTries;

                try
                {
                    var fileOptions = FileOptions.None;

                    if (DirectorySettings.DeleteProcessedFile) 
                    {
                        fileOptions = FileOptions.DeleteOnClose;
                    }
                        
                    // Занимаем файл, чтобы никто не мог с ним ничего сделать, пока не закончим
                    using (FileStream fileStream =
                           new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, 4096, fileOptions))
                    {
                        Dictionary<string, bool> directoriesExist = CheckDirectoriesExist(out bool allDirectoriesAvailable);

                        if (!(DirectorySettings.IgnoreUnavailableDirectory || allDirectoriesAvailable))
                        {
                            string unavailableDirectories = string.Join("\r\n", directoriesExist.Where(kvp => !kvp.Value).Select(kvp => string.Format("[{0}]", kvp.Key)));

                            LoggerModel.Error($"Пути для копирования недоступны:\r\n{unavailableDirectories}");

                            return;
                        }

                        MoveFileToPaths(filePath, directoriesExist);
                        break;
                    }
                }
                catch (Exception)
                {
                    LoggerModel.Warn($"Файл {filePath} занят другим процессом. Попытка копировать файл {numTries} из {DirectorySettings.MaxTries}");
                    if (numTries > DirectorySettings.MaxTries)
                    {
                        if (RestartsCount > DirectorySettings.MaxRestartCount)
                        {
                            CorruptedFiles.Add(filePath, lastWriteDate);
                            LoggerModel.Warn($"Файл {filePath} с датой изменения {lastWriteDate} помещен в игнор.");
                            RestartsCount = 0;
                        }

                        throw;
                    }
                    Thread.Sleep(1000);
                }
            }

            //Если работаем сканером - добавить путь и время записи обработанного файла в список
            if (0 < DirectorySettings.Timeout)
            {
                if (!ProcessedFiles.ContainsKey(filePath))
                {
                    ProcessedFiles.Add(filePath, lastWriteDate);
                }
                else 
                {
                    ProcessedFiles[filePath] = lastWriteDate;
                }
            }
            DataWarehouseModel.AddUILog(new UILogModel($"Processing complete: [{filePath}]", DataWarehouseModel.ImportanceLogs.low));

            LoggerModel.Info($"Processing complete: [{filePath}]");
        }

        private Dictionary<string, bool> CheckDirectoriesExist(out bool allDirectoriesAvailable)
        {
            Dictionary<string, bool> directoryExist = new Dictionary<string, bool>();

            foreach (var destination in DirectorySettings.MoveToPaths)
            {
                directoryExist.Add(destination, Directory.Exists(destination));
            }
            allDirectoriesAvailable = !directoryExist.Any(kvp => !kvp.Value);

            return directoryExist;
        }

        private void MoveFileToPaths(string filePath, Dictionary<string, bool> directoriesExist)
        {
            foreach (var destination in DirectorySettings.MoveToPaths)
            {
                if (!directoriesExist[destination])
                {
                    DataWarehouseModel.AddUILog(new UILogModel($"Directory: [{destination}] - not found", DataWarehouseModel.ImportanceLogs.low));

                    LoggerModel.Error($"Directory: [{destination}] - not found");

                    continue;
                }

                string name = Path.GetFileName(filePath);

                string dest = destination + "\\" + name;

                if (File.Exists(dest))
                {
                    string nameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                    string ext = Path.GetExtension(filePath);

                    dest = $"{destination}\\{nameWithoutExtension}.{Guid.NewGuid()}{ext}";
                }

                File.Copy(filePath, dest, true);
            }
            DataWarehouseModel.AddUILog(new UILogModel($"File was coppied success: [{filePath}]", DataWarehouseModel.ImportanceLogs.low));

            LoggerModel.Info($"File was coppied success: [{filePath}]");
        }

        /// <summary>
        ///     Уничтожение наблюдателя
        /// </summary>
        /// <returns></returns>
        public void Dispose()
        {
            Dispose();

            GC.SuppressFinalize(this);
        }

    }
}
