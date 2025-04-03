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
                { CommonSettings.LoadSettings(); }));
            }
        }
        public ApplicationViewModel() 
        {
            ListBoxLog = new ListBox();

            ListBoxLogTemp = new ListBox();

            //CommonSettings.Instance.WindowToShow += CommonSettingsEventHandler;

            UILogHandlerModel.LogIsUpdated += (sender, e) =>
            {
                var itemLog = CreateListBoxItem(e);

                if (ListBoxLogTemp != null) 
                {
                    ListBoxItem logItem = new ListBoxItem();

                    ListBoxLogTemp.Items.Add(logItem);

                    ListBoxLog = ListBoxLogTemp;
                }
            };
        }
        public void FooTest() 
        {
            DirectorySettingsWindow directorySettingsWindow = new DirectorySettingsWindow();

            WindowToShow.Invoke(directorySettingsWindow, "12343");
        }

        public void CommonSettingsEventHandler(Window window, string message)
        {
            WindowToShow?.Invoke(window, message);
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
        public delegate void WindowMessageHandler(Window window, string message);

        public event WindowMessageHandler WindowToShow;
    }
}
