using Autofac;
using FileTransferWpfApp.Tools;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FileTransferWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DependencyConfig.Configure();

            var mainWindow = DependencyConfig.Container.Resolve<MainWindow>();

            mainWindow.Show();
        }
    }
}
