using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceProcess;
using NLog;
using System.Diagnostics.Metrics;
using FileTransferWpfApp.ViewModel;
using FileTransferWpfApp.View.UserView;
using FileTransferWpfApp.Model.ModelHandlers;
using FileTransferWpfApp.Model.ModelLogs;
using FileTransferWpfApp.Model.ModelSettings;

namespace FileTransferWpfApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationViewModel appViewModel;


        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public static ListBox? ListBoxLog { get; set; }
        private List<FileWatcher> Watchers { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ListBoxLog = listBoxLog;

            DataWarehouse.AllocateMemory();

            Watchers = [];

            btnStart.IsEnabled = false;

            BtnStartTransferClick();
        }
        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            DirectorySettingsWindow directorySettingsWindow = new DirectorySettingsWindow();

            directorySettingsWindow.Show();
        }

        private async void BtnStartTransferClick(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;

            await appViewModel.StartPoint();
        }
        private async void BtnStartTransferClick() 
        {
            await appViewModel.StartPoint();
        }
        private void btnTestTransferClick(object sender, RoutedEventArgs e)
        {
            var testingTransferMainWindow = new TestingTransferMainWindow();

            testingTransferMainWindow.Show();
        }
    }
}