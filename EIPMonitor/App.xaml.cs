﻿using EIPMonitor.Ioc;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Views.UserWindowViews;
using Ninject;
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
            IocKernel.Initialize(new IocConfiguration());
            base.OnStartup(e);
        }
    }
}
