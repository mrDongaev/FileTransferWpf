using Autofac;
using FileTransferWpfApp.Tools;
using FileTransferWpfApp.Tools.Factories.Managers;
using FileTransferWpfApp.Tools.Factories;
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
            CommonSettingsFactory commonSettingsFactory = new CommonSettingsFactory(/* передайте необходимые зависимости */);
            CommonSettingsManager commonSettingsManager = new CommonSettingsManager(commonSettingsFactory);
        }
    }
}
