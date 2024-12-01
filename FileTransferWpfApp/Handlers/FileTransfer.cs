using FileTransferWpf.Data.Entity;
using FileTransferWpf.Tools.Entity;
using FileTransferWpf.Tools;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileTransferWpfApp.Handlers;

namespace FileTransferWpf.Handlers
{
    public class FileTransfer
    {
        private DirectorySettings DirectorySettings { get; set; }

        private readonly Timer _timer;

        /// <summary>
        /// Запустить наблюдатели за файлами
        /// </summary>
        public FileTransfer(DirectorySettings directorySettings)
        {
            DirectorySettings = directorySettings ?? throw new ArgumentNullException(nameof(directorySettings));

            DataWarehouse.AddAndUpdateLogInterface(new ScreenLog(
                $"Запуск наблюдателя за файлами\n Директория для {directorySettings.DeviceName} \n Общее число настроек директорий {CommonSettings.Instance.Directories.Count}",
                DataWarehouse.ImportanceLogs.low));

            // Инициализация таймера для периодического обновления
            _timer = new Timer(PassiveFileWatcher, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        }

        public async Task StartTransfer()
        {
            await ScannerDirectory();
        }

        private async Task ScannerDirectory()
        {
            while (true)
            {
                try
                {
                    InterfaceLogHandler.RefreshLog();

                    if (!Directory.Exists(DirectorySettings.MoveFromPath))
                    {
                        Thread.Sleep(2000);

                        throw new DirectoryNotFoundException(DirectorySettings.MoveFromPath);
                    }

                    int countFiles = Directory.GetFiles(DirectorySettings.MoveFromPath).Count();

                    if (countFiles > 0)
                    {
                        DataWarehouse.AddAndUpdateLogInterface(new ScreenLog(
                            $"Найдено {countFiles} в директории {DirectorySettings.MoveFromPath}",
                            DataWarehouse.ImportanceLogs.low));
                        await TransferFiles();
                    }
                }
                catch (Exception ex)
                {
                    DataWarehouse.AddAndUpdateLogInterface(new ScreenLog(ex.ToString(), DataWarehouse.ImportanceLogs.high));
                }

                // Ожидание перед следующей итерацией
                await Task.Delay(2000);
            }
        }

        private async Task TransferFiles()
        {
            string sourcePath = DirectorySettings.MoveFromPath;

            try
            {
                foreach (var targetPath in DirectorySettings.MoveToPaths)
                {
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }

                    var files = Directory.GetFiles(sourcePath);

                    foreach (var file in files)
                    {
                        string fileName = Path.GetFileName(file);

                        string destFile = Path.Combine(targetPath, fileName);

                        File.Copy(file, destFile, true);

                        DataWarehouse.AddAndUpdateLogInterface(new ScreenLog(
                            $"Файл {fileName} скопирован в {targetPath}",
                            DataWarehouse.ImportanceLogs.low));

                        // Удаление файла после копирования
                        File.Delete(file);

                        DataWarehouse.AddAndUpdateLogInterface(new ScreenLog(
                            $"Файл {fileName} удален из {sourcePath}",
                            DataWarehouse.ImportanceLogs.medium));
                    }
                }
            }
            catch (Exception ex)
            {
                DataWarehouse.AddAndUpdateLogInterface(new ScreenLog($"Ошибка копирования файлов: {ex.Message}", DataWarehouse.ImportanceLogs.high));
            }

            await Task.CompletedTask;
        }

        private void PassiveFileWatcher(object state)
        {
            // Логика для пассивного наблюдателя за файлами
            // Здесь можно добавить код для перезапуска наблюдателей или другой логики
        }
    }
}