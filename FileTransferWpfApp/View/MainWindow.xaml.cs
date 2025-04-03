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
using System.Security.Cryptography.X509Certificates;

namespace FileTransferWpfApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            
            viewModel = new ApplicationViewModel();

            viewModel.WindowToShow += ShowTempWindow;

            DataContext = viewModel;

            viewModel.LoadSettingsCommand.Execute(this);
        }
        public static void ShowTempWindow(Window window, string message) 
        {
            window.Show();
        }

        private void btnTestTransfer_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}