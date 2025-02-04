using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferWpfApp.Model.ModelLogs
{
    public class UILogModel
    {
        public DataWarehouseModel.ImportanceLogs ImportanceLogs { get; set; }
        public string Message { get; set; }

        public UILogModel() { }

        public UILogModel(string message, DataWarehouseModel.ImportanceLogs importanceLogs) 
        {
            Message = message;

            ImportanceLogs = importanceLogs;
        }
    }
}
