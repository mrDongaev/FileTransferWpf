using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTransferWpf.Tools;

namespace FileTransferWpfApp.Tools.Factories
{
    public class CommonSettingsFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        private CommonSettings _commonSettingsInstance;

        public CommonSettingsFactory(ILifetimeScope lifetimeScope) 
        {
            _lifetimeScope = lifetimeScope;
        }

        public CommonSettings GetCommonSettings() 
        {
            return _lifetimeScope.Resolve<CommonSettings>();
        }
    }
}
