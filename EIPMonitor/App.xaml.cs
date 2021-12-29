using EIPMonitor.IDomainService;
using EIPMonitor.Ioc;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.ViewDialog;
using EIPMonitor.Views.UserWindowViews;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EIPMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IocConfiguration iocConfiguration = new IocConfiguration();
            iocConfiguration.InitializeModules();
            var dialog = IocKernel.Get<IModelDialog>("LoginViewDialog");
            dialog.BindDefaultViewModel();
            dialog.ShowDialog();

        }
    }
}
