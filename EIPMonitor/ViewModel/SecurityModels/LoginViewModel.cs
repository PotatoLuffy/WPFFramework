using EIPMonitor.DomainServices.UserService;
using EIPMonitor.IDomainService;
using EIPMonitor.Ioc;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.ViewModel.CustomException;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EIPMonitor.ViewModel.SecurityModels
{
    public class LoginViewModel:ViewModelBase
    {
        private EIPProductionIndexUsers eIPProductionIndexUsers;
        private string _name;
        private string _password;
        private IEIPProductionIndexUsersLogin iEIPProductionIndexUsersLogin;
        public LoginViewModel()
        {
            eIPProductionIndexUsers = new EIPProductionIndexUsers();
            iEIPProductionIndexUsersLogin = IocKernel.Get<IEIPProductionIndexUsersLogin>();
        }

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private RelayCommand _exitCommand;

        public RelayCommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand(() => ApplicationShutdown());
                }
                return _exitCommand;
            }
        }
        public EIPProductionIndexUsers User
        {
            get => eIPProductionIndexUsers;
            set
            {
                SetProperty<EIPProductionIndexUsers>(ref eIPProductionIndexUsers, value);
            }
        }
        public bool CanLogin()
        {
            return !String.IsNullOrEmpty(this._password) && !String.IsNullOrEmpty(this._name);
        }
        public void Login()
        {
            eIPProductionIndexUsers.UserName = this._name;
            eIPProductionIndexUsers.Password = this._password;
        }
        private void ApplicationShutdown()
        {
            Messenger.Default.Send(String.Empty, "ApplicationShutdown");
        }
        public async Task LoginAction()
        {
            try
            {
                this.Login();
                Task<EIPProductionIndexUsers> resultTask = null;
                //var resultTask = iEIPProductionIndexUsersLogin.Login(this.User);
                //resultTask.Wait();
                //if (resultTask.Result == null)
                //{
                //    throw new LoginAuthenticatedFailureException("用户名或者密码错误。");
                //    //Messenger.Default.Send("用户名或者密码错误。", "SendMessageToLoginWin");
                //}
                var userStamp = IocKernel.Get<IUserStamp>();
                userStamp.UserName = resultTask?.Result.UserName??"123";
                userStamp.EmployeeId = resultTask?.Result.EmployeeId??"123";
                //Messenger.Default.Send("验证成功准备登录。", "SendMessageToLoginWin");
                
                var dialog = IocKernel.Get<IModelDialog>("MainWindowDialog");
                dialog.BindDefaultViewModel();
                Messenger.Default.Send(String.Empty, "ApplicationHiding");
                var mainWin = dialog.ShowDialog();
                //Messenger.Default.Send("开始加载", "SendMessageToMainWin");
                
                this.ApplicationShutdown();
            }
            catch (Exception e)
            {
               throw new Exception("网络链接失败,请检查自己网络和服务器网络是否畅通。");
            }
        }
    }
}
