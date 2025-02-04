using FileTransferWpfApp.Commands;
using FileTransferWpfApp.Model.ModelLogs;
using FileTransferWpfApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferWpfApp.ViewModel
{
    public class DataWarehouseViewModel
    {
        DataWarehouse _dataWarehouse;
        public DataWarehouseViewModel() 
        {
            _dataWarehouse = new DataWarehouse();

            _dataWarehouse.ScreenLogAdded += DataWarehouse_ScreenLogAdded;
        }
        public static void AddAndUpdateLogInterface(ScreenLog screenLog)
        {
            screenLogs.Add(screenLog);

            UiLogHandler.RefreshLog();
        }
        public static ScreenLog[] InArray()
        {
            return screenLogs.ToArray();
        }
        public static List<ScreenLog> AllocateMemory()
        {
            screenLogs = [];

            return screenLogs;
        }

        public RelayCommand AddLogOnUI
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(async obj =>
                {

                }));
            }
        }

        private void DataWarehouse_ScreenLogAdded(object? sender, ScreenLog e)
        {
            throw new NotImplementedException();
        }
    }
}
