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
using FileTransferWpf.Tools;
using FileTransferWpf.Tools.Entity;
using FileTransferWpf.Handlers;
using FileTransferWpf.Data.Entity;
using NLog;
using System.Diagnostics.Metrics;
using FileTransferWpfApp.UserInterfaceClasses.UserWindows;

namespace FileTransferWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public static ListBox? ListBoxLog { get; set; }
        private List<FileWatcher> Watchers { get; set; }
        private Thread MainThread { get; }
        public MainWindow()
        {
            InitializeComponent();

            ListBoxLog = listBoxLog;

            DataWarehouse.screenLogs = [];

            Watchers = [];
        }
        public async Task StartPoint()
        {
            try
            {
                DataWarehouse.screenLogs.Add(new ScreenLog("Приложение запущено в форме WPF", DataWarehouse.ImportanceLogs.low));

                if (CommonSettings.LoadSettings())
                {
                    // Создаем список задач для параллельного выполнения
                    var tasks = new List<Task>();

                    foreach (var dirSets in CommonSettings.Instance.Directories)
                    {
                        FileTransfer fileTransfer = new(dirSets);

                        // Добавляем задачу в список
                        tasks.Add(fileTransfer.StartTransfer());
                    }
                    // Ожидаем завершения всех задач
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                // Логирование исключений
                DataWarehouse.screenLogs.Add(new ScreenLog($"Ошибка в StartPoint: {ex.Message}", DataWarehouse.ImportanceLogs.high));
                throw;
            }
        }

        private void BtnExitClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private async void BtnStartTransferClick(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;

            await StartPoint();
        }
        private void ListBoxLogMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxLog?.SelectedItem is ListBoxItem selectedItem)
            {
                var logMessage = selectedItem.Content.ToString();

                MessageBox.Show(logMessage, "Log Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btnTestTransferClick(object sender, RoutedEventArgs e)
        {
            var testingTransferMainWindow = new TestingTransferMainWindow();

            testingTransferMainWindow.Show();
        }
    }
}