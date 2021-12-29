using EIPMonitor.DomainServices.UserService;
using EIPMonitor.IDomainService;
using EIPMonitor.Ioc;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
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
        private int loginButtonCnt = 0;
        public LoginViewModel()
        {
            eIPProductionIndexUsers = new EIPProductionIndexUsers();
            iEIPProductionIndexUsersLogin = IocKernel.Get<IEIPProductionIndexUsersLogin>();
            Messenger.Default.Send("开始检查", "SendMessageToLoginWin");
        }

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private RelayCommand _signCommand;
        public RelayCommand SignCommand
        {
            get
            {
                if (_signCommand == null)
                {
                    _signCommand = new RelayCommand(()=>LoginAction());
                }
                return _signCommand;
            }
        }
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
            return !String.IsNullOrEmpty(this._password) && !String.IsNullOrEmpty(this._name) && loginButtonCnt == 0;
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
        public void LoginAction()
        {
            Interlocked.Increment(ref loginButtonCnt);
            try
            {
                this.Login();
                //var result = await iEIPProductionIndexUsersLogin.Login(this.User).ConfigureAwait(false);
                //if (result == null)
                //{
                //    Messenger.Default.Send("用户名或者密码错误。", "SendMessageToLoginWin");
                //    return;
                //}
                EIPProductionIndexUsers result = null;
                var userStamp = IocKernel.Get<IUserStamp>();
                userStamp.UserName = result?.UserName??"123";
                userStamp.EmployeeId = result?.EmployeeId??"123";
                //Messenger.Default.Send("验证成功准备登录。", "SendMessageToLoginWin");
                
                var dialog = IocKernel.Get<IModelDialog>("MainWindowDialog");
                dialog.BindDefaultViewModel();
                Messenger.Default.Send(String.Empty, "ApplicationHiding");
                var resultTask = dialog.ShowDialog();
                //Messenger.Default.Send("开始加载", "SendMessageToMainWin");
                
                this.ApplicationShutdown();
            }
            catch (Exception e)
            {
                Messenger.Default.Send("网络链接失败,请检查自己网络和服务器网络是否畅通。", "SendMessageToLoginWin");
            }
            finally
            {
                Interlocked.Decrement(ref loginButtonCnt);
            }
        }
    }
}
