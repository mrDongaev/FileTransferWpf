using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FileTransferWpf.Tools;
using FileTransferWpfApp.Tools.DIServices;

namespace FileTransferWpfApp.Tools
{
    public class DependencyConfig
    {
        public static IContainer Container { get; private set; }
        public static void Configure() 
        {
            var builder = new ContainerBuilder();

            //Регистрация DirectorySettings как singleton
            builder.RegisterType<CommonSettings>().SingleInstance();

            builder.RegisterType<CommonSettingsService>().AsSelf();

            Container = builder.Build();

            var commonSettingsService = Container.Resolve<CommonSettingsService>();

            CommonSettings commonSettings = commonSettingsService.GetCommonSettings();
        }
    }
}
