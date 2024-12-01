using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTransferWpf.Tools.Entity;
using FileTransferWpfApp.Handlers;

namespace FileTransferWpf.Data.Entity
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
