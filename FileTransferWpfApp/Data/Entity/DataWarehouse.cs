using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTransferWpf.Tools.Entity;

namespace FileTransferWpf.Data.Entity
{
    public static class DataWarehouse
    {
        public static List<ScreenLog> screenLogs;

        public enum ImportanceLogs { high, medium, low }
    }
}
