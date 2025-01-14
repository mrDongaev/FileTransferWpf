using FileTransferWpfApp.Commands;
using FileTransferWpfApp.Model.ModelHandlers;
using FileTransferWpfApp.Model.ModelLogs;
using FileTransferWpfApp.Model.ModelSettings;
using FileTransferWpfApp.View.UserView;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileTransferWpfApp.ViewModel
{
    public class ApplicationViewModel
    {
        private CommonSettings commonSettings;

        //Команда загрузки настроек
        private RelayCommand addCommand;

        public RelayCommand LoadSettingsCommand 
        {
            get 
            {
                return addCommand ?? (addCommand = new RelayCommand(async obj => 
                { await CommonSettings.LoadSettings(); }));
            }
        }
        public RelayCommand ChangeSettingsCommand 
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(async obj =>
                { await CommonSettings.LoadSettings(); }));
            }
        }
        public RelayCommand ShowWindow 
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(async obj =>
                {
                    if (obj is Window) 
                    {
                        var window = (Window)obj;

                        window.Show();
                    }
                }));
            }
        }
        public ApplicationViewModel() 
        {
            commonSettings = new CommonSettings();

            commonSettings.MessageToShow += (sender, message) =>
            {
                MessageBox.Show(message);
            };
        }
        //public async Task StartPoint()
        //{
        //    try
        //    {
        //        DataWarehouse.AddAndUpdateLogInterface(new ScreenLog("Приложение запущено в форме WPF", DataWarehouse.ImportanceLogs.low));

        //        if (await CommonSettings.LoadSettings())
        //        {
        //            // Создаем список задач для параллельного выполнения
        //            var tasks = new List<Task>();

        //            foreach (var dirSets in CommonSettings.Instance.Directories)
        //            {
        //                FileTransfer fileTransfer = new(dirSets);

        //                // Добавляем задачу в список
        //                tasks.Add(fileTransfer.StartTransfer());
        //            }
        //            // Ожидаем завершения всех задач
        //            await Task.WhenAll(tasks);
        //        }
        
        //    }
        //    catch (Exception ex)
        //    {
        //        // Логирование исключений
        //        DataWarehouse.AddAndUpdateLogInterface(new ScreenLog($"Ошибка в StartPoint: {ex.Message}", DataWarehouse.ImportanceLogs.high));
        //        throw;
        //    }
        //}
    }
}
