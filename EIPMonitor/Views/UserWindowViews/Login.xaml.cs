using EIPMonitor.DomainServices.UserService;
using EIPMonitor.Ioc;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.ViewModel.SecurityModels;
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
        private LoginViewModel loginViewModel;
        private IEIPProductionIndexUsersLogin iEIPProductionIndexUsersLogin;
        public Login()
        {
            InitializeComponent();
            loginViewModel = new LoginViewModel();
            iEIPProductionIndexUsersLogin = IocKernel.Get<IEIPProductionIndexUsersLogin>();
            this.DataContext = loginViewModel;
        }

        private ICommand _signCommand;
        public ICommand SignCommand
        {
            get
            {
                if (_signCommand == null)
                {
                    _signCommand = new CommandHandler(() => LoginAction(), () => CanLogin());
                }
                return _signCommand;
            }
        }
        //public EIPProductionIndexUsers User
        //{ 
        //    get => eIPProductionIndexUsers; 
        //    set { 
        //        SetProperty<EIPProductionIndexUsers>(ref eIPProductionIndexUsers, value); 
        //    } 
        //}
        public bool CanLogin()
        {
            return !String.IsNullOrEmpty(this.loginViewModel.Name) && !String.IsNullOrEmpty(this.loginViewModel.Password);
        }
        public void LoginAction()
        {
            this.loginViewModel.Login();
            var task = iEIPProductionIndexUsersLogin.Login(this.loginViewModel.User);
            task.Wait();

            if (task.Result == null)
            {
                MainSnackbar.MessageQueue?.Enqueue($"登录失败，用户名或密码错误。");
                return;
            }
            var userStamp = IocKernel.Get<IUserStamp>();
            userStamp.UserName = task.Result.UserName;
            userStamp.EmployeeId = task.Result.EmployeeId;
            //note you can use the message queue from any thread, but just for the demo here we 
            //need to get the message queue from the snackbar, so need to be on the dispatcher
            MainSnackbar.MessageQueue?.Enqueue($"用户名:{userStamp.UserName},工号:{userStamp.EmployeeId} 登录成功");
            this.Close();

        }
        private void pass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.loginViewModel.Password = this.pass.Password;
        }

        private void PackIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Application.Current.Shutdown();
        }
    }
}
