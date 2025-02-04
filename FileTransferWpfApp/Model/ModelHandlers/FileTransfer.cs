using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileTransferWpfApp.Model.ModelSettings;
using FileTransferWpfApp.Model.ModelLogs;

namespace FileTransferWpfApp.Model.ModelHandlers
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

            DataWarehouseModel.AddUILog(new UILogModel(
                $"Запуск наблюдателя за файлами\n Директория для {directorySettings.DeviceName} \n Общее число настроек директорий {CommonSettings.Instance.Directories.Count}",
                DataWarehouseModel.ImportanceLogs.low));

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
                    if (!Directory.Exists(DirectorySettings.MoveFromPath))
                    {
                        Thread.Sleep(2000);

                        throw new DirectoryNotFoundException(DirectorySettings.MoveFromPath);
                    }

                    int countFiles = Directory.GetFiles(DirectorySettings.MoveFromPath).Count();

                    if (countFiles > 0)
                    {
                        DataWarehouseModel.AddUILog(new UILogModel(
                            $"Найдено {countFiles} в директории {DirectorySettings.MoveFromPath}",
                            DataWarehouseModel.ImportanceLogs.low));
                        await TransferFiles();
                    }
                }
                catch (Exception ex)
                {
                    DataWarehouseModel.AddUILog(new UILogModel(ex.ToString(), DataWarehouseModel.ImportanceLogs.high));
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

                        DataWarehouseModel.AddUILog(new UILogModel(
                            $"Файл {fileName} скопирован в {targetPath}",
                            DataWarehouseModel.ImportanceLogs.low));

                        // Удаление файла после копирования
                        File.Delete(file);

                        DataWarehouseModel.AddUILog(new UILogModel(
                            $"Файл {fileName} удален из {sourcePath}",
                            DataWarehouseModel.ImportanceLogs.medium));
                    }
                }
            }
            catch (Exception ex)
            {
                DataWarehouseModel.AddUILog(new UILogModel($"Ошибка копирования файлов: {ex.Message}", DataWarehouseModel.ImportanceLogs.high));
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