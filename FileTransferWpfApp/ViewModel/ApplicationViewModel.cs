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
using System.Runtime.CompilerServices;
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

        ListBox ListBoxLogTemp { get; set; }

        private ListBox listBoxLog;

        public ListBox ListBoxLog
        {
            get { return listBoxLog; }
            set
            {
                listBoxLog = value;
                OnPropertyChanged(nameof(ListBoxLog));
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
        public ApplicationViewModel() 
        {
            commonSettings = new CommonSettings();

            ListBoxLog = new ListBox();

            ListBoxLogTemp = new ListBox();

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
                var itemLog = CreateListBoxItem(e);

                if (ListBoxLogTemp != null) 
                {
                    //ListBoxLogTemp.Items.Add(itemLog);

                    ListBoxItem logItem = new ListBoxItem();

                    logItem.Content = "Вот так";

                    ListBoxLogTemp.Items.Add(logItem);

                    ListBoxLog = ListBoxLogTemp;
                }
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
        //        DataWarehouseModel.AddUILog(new ScreenLog("Приложение запущено в форме WPF", DataWarehouseModel.ImportanceLogs.low));

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
        //        DataWarehouseModel.AddUILog(new ScreenLog($"Ошибка в StartPoint: {ex.Message}", DataWarehouseModel.ImportanceLogs.high));
        //        throw;
        //    }
        //}
    }
}
