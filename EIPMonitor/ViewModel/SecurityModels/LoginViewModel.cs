using EIPMonitor.Ioc;
using EIPMonitor.Model;
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
        public LoginViewModel()
        {
            eIPProductionIndexUsers = new EIPProductionIndexUsers();
        }

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }

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
    }
}
