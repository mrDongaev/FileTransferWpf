using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferWpfApp.Model.ModelLogs
{
    public static class DataWarehouse
    {
        private static List<ScreenLog> screenLogs;

        public enum ImportanceLogs { high, medium, low }

        public static void AddAndUpdateLogInterface(ScreenLog screenLog) 
        {
            screenLogs.Add(screenLog);

            InterfaceLogHandler.RefreshLog();
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
    }
}
