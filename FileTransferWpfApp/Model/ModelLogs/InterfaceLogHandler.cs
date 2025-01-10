using FileTransferWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FileTransferWpfApp.ViewModel;

namespace FileTransferWpfApp.Model.ModelLogs
{
    public class InterfaceLogHandler
    {
        public InterfaceLogHandler() { }

        public static void RefreshLog()
        {
            try
            {
                MainWindow.ListBoxLog?.Items.Clear();

                var screenLogsTemp = DataWarehouse.InArray();

                foreach (var item in screenLogsTemp)
                {
                    var listBoxItem = CreateListBoxItem(item);

                    MainWindow.ListBoxLog?.Items.Add(listBoxItem);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private static ListBoxItem CreateListBoxItem(ScreenLog log)
        {
            var listBoxItem = new ListBoxItem { Content = log.Message };

            listBoxItem.Background = log.ImportanceLogs switch
            {
                DataWarehouse.ImportanceLogs.low => Brushes.LightGreen,
                DataWarehouse.ImportanceLogs.high => Brushes.OrangeRed,
                DataWarehouse.ImportanceLogs.medium => Brushes.Yellow,
                _ => listBoxItem.Background
            };

            return listBoxItem;
        }
    }
}
