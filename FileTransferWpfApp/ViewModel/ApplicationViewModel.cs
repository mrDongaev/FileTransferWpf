using FileTransferWpfApp.Commands;
using FileTransferWpfApp.Model.ModelHandlers;
using FileTransferWpfApp.Model.ModelLogs;
using FileTransferWpfApp.Model.ModelSettings;
using FileTransferWpfApp.View.UserView;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace FileTransferWpfApp.ViewModel
{
    public class ApplicationViewModel : ViewModelBase
    {
        private CommonSettings commonSettings;

        private ListBoxItem logMessage;

        public ListBoxItem LogMessage 
        {
            get { return logMessage; }

            set 
            {
                logMessage = value;

                OnPropertyChanged(nameof(LogMessage));
            }
        }

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
            commonSettings.WindowToShow += (sender, window) =>
            {
                window.Show();
            };
            UILogHandlerModel.LogIsUpdated += (sender, e) =>
            {
                LogMessage = CreateListBoxItem(e);
            };

        }
        private static ListBoxItem CreateListBoxItem(UILogModel log)
        {
            var listBoxItem = new ListBoxItem { Content = log.Message };

            listBoxItem.Background = log.ImportanceLogs switch
            {
                DataWarehouseModel.ImportanceLogs.low => Brushes.LightGreen,
                DataWarehouseModel.ImportanceLogs.high => Brushes.OrangeRed,
                DataWarehouseModel.ImportanceLogs.medium => Brushes.Yellow,
                _ => listBoxItem.Background
            };

            return listBoxItem;
        }
        //public async Task StartPoint()
        //{
        //    try
        //    {
        //        DataWarehouse.AddUILog(new ScreenLog("Приложение запущено в форме WPF", DataWarehouse.ImportanceLogs.low));

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
        //        DataWarehouse.AddUILog(new ScreenLog($"Ошибка в StartPoint: {ex.Message}", DataWarehouse.ImportanceLogs.high));
        //        throw;
        //    }
        //}
    }
}
