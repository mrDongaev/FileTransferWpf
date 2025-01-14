using FileTransferWpfApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferWpfApp.Model.ModelLogs
{
    public class DataWarehouse
    {
        private List<ScreenLog> screenLogs;

        public enum ImportanceLogs { high, medium, low }

        private static DataWarehouse? instance;
        
        public static DataWarehouse? Instance 
        {
            get 
            {
                if (instance == null)
                {
                    instance = new DataWarehouse();
                }
                return instance;
            }
        }
        public event EventHandler<ScreenLog> ScreenLogAdded;

        public void OnScreenLogAdded(ScreenLog screenLog) 
        {
            ScreenLogAdded?.Invoke(this, screenLog);
        }
    }
}
