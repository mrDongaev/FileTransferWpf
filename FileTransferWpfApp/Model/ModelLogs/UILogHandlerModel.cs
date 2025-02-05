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
using System.Runtime.CompilerServices;

namespace FileTransferWpfApp.Model.ModelLogs
{
    public class UILogHandlerModel
    {
        public UILogHandlerModel() { }

        private static UILogHandlerModel instance;
        public static UILogHandlerModel Instance 
        {
            get 
            {
                if (instance == null) 
                    instance = new UILogHandlerModel();
                return instance;
            }
        }

        public static async void RefreshLog()
        {
            try
            {
                var uILogs = DataWarehouseModel.InArray();

                var endingIndex = uILogs.Length; 

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    LogUpdate(uILogs[endingIndex-1]);
                });
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public static event EventHandler<UILogModel> LogIsUpdated;

        public static void LogUpdate(UILogModel messageLog) 
        {
            LogIsUpdated?.Invoke(instance, messageLog);
        }
    }
}
