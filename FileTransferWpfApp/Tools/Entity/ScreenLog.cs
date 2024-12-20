﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTransferWpf.Data.Entity;

namespace FileTransferWpf.Tools.Entity
{
    public class ScreenLog
    {
        public DataWarehouse.ImportanceLogs ImportanceLogs { get; set; }
        public string Message { get; set; }

        public ScreenLog() { }

        public ScreenLog(string message, DataWarehouse.ImportanceLogs importanceLogs) 
        {
            Message = message;

            ImportanceLogs = importanceLogs;
        }
    }
}
