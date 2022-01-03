using EIPMonitor.CustomUserControlRepository;
using EIPMonitor.DomainServices.MasterData;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.ViewModel.MessagesViewModels;
using EIPMonitor.ViewModel.NavigationBar;
using EIPMonitor.Views.UserWindowViews;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EIPMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar;
        public MainWindow()
        {
            InitializeComponent();
            Snackbar = this.MainSnackbar;
            // eIP_PRO_GlobalParamConfigureService = IocKernel.Get<IEIP_PRO_GlobalParamConfigureService>();
            //mainWindowViewModel = new MainWindowViewModel();
            //DataContext = mainWindowViewModel;
            //Mutex myMutex = new Mutex(true, "EIPMonitor", out ifNewInstance);
            //if (!ifNewInstance)
            // {
            //    this.Close();
            //    Environment.Exit(0);
            //    return;
            //}
            //login = new Login();
            //login.Show();
            //login.Closed += LoginWinClosed_EventHandler;
            ///DataContext = new MainWindowViewModel(MainSnackbar.MessageQueue!);
        }

        private void OnSelectedItemChanged(object sender, DependencyPropertyChangedEventArgs e)=> MainScrollViewer.ScrollToHome();
        private async void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new SampleMessageDialog
            {
                Message = { Text = ((ButtonBase)sender).Content.ToString() },
            };

            await DialogHost.Show(sampleMessageDialog, "RootDialog");
        }
        private async void changePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new SampleMessageDialog
            {
                Message = { Text = ((ButtonBase)sender).Content.ToString() },

            };
            await DialogHost.Show(sampleMessageDialog, "RootDialog");
        }
    }
}
