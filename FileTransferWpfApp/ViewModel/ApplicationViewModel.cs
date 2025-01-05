using FileTransferWpfApp.Commands;
using FileTransferWpfApp.Model.ModelHandlers;
using FileTransferWpfApp.Model.ModelLogs;
using FileTransferWpfApp.Model.ModelSettings;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FileTransferWpfApp.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private CommonSettings commonSettings;

        //Команда загрузки настроек
        private RelayCommand loadSettingsCommand;

        public RelayCommand LoadSettingsCommand 
        {
            get 
            {
                return loadSettingsCommand ?? (loadSettingsCommand = new RelayCommand(obj => 
                { CommonSettings.LoadSettings() }));
            }
        }

        public ApplicationViewModel() 
        {
            commonSettings = new CommonSettings();
        }
        public async Task StartPoint()
        {
            try
            {
                DataWarehouse.AddAndUpdateLogInterface(new ScreenLog("Приложение запущено в форме WPF", DataWarehouse.ImportanceLogs.low));

                if (await CommonSettings.LoadSettings())
                {
                    // Создаем список задач для параллельного выполнения
                    var tasks = new List<Task>();

                    foreach (var dirSets in CommonSettings.Instance.Directories)
                    {
                        FileTransfer fileTransfer = new(dirSets);

                        // Добавляем задачу в список
                        tasks.Add(fileTransfer.StartTransfer());
                    }
                    // Ожидаем завершения всех задач
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                // Логирование исключений
                DataWarehouse.AddAndUpdateLogInterface(new ScreenLog($"Ошибка в StartPoint: {ex.Message}", DataWarehouse.ImportanceLogs.high));
                throw;
            }
        }
    }
}
