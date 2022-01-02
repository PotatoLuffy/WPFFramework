using EIPMonitor.DomainService;
using EIPMonitor.DomainServices.UserService;
using EIPMonitor.Ioc;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.ViewModel.SecurityModels;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EIPMonitor.Views.UserWindowViews
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private IRequestLimitControlService requestLimitControlService;
        private readonly string className;
        public Login()
        {
            InitializeComponent();
            requestLimitControlService = IocKernel.Get<IRequestLimitControlService>();
            className = this.GetType().FullName;
        }


        //public EIPProductionIndexUsers User
        //{ 
        //    get => eIPProductionIndexUsers; 
        //    set { 
        //        SetProperty<EIPProductionIndexUsers>(ref eIPProductionIndexUsers, value); 
        //    } 
        //}

        private void pass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (this.DataContext as LoginViewModel).Password = this.pass.Password;
        }

        private void PackIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Application.Current.Shutdown();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string btnName = button.Name;
            var getThePermission = requestLimitControlService.RequestClickPermission(className, btnName);
            if (!getThePermission)
            {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToLoginWin");
                return;
            }
            var dataContext = this.DataContext as LoginViewModel;
             await dataContext?.LoginAction().ContinueWith(t=> {
                requestLimitControlService.ReleaseClickPermission(this.GetType().FullName, btnName);
                if (t.IsFaulted)
                    Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToLoginWin");              
                    },TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
