using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferWpfApp.Model.ModelLogs
{
    public static class DataWarehouseModel
    {
        private static List<UILogModel>? uILogs;

        public enum ImportanceLogs { high, medium, low }

        public static void AddUILog(UILogModel screenLog) 
        {
            uILogs?.Add(screenLog);

            UILogHandlerModel.RefreshLog();
        }
        public static UILogModel[] InArray() 
        {
            return uILogs.ToArray();
        }
        public static List<UILogModel> AllocateMemory() 
        {
            uILogs = [];

            return uILogs;
        }
    }
}
