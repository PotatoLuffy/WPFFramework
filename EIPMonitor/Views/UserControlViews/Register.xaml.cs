using EIPMonitor.DomainServices.UserService;
using EIPMonitor.Model;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EIPMonitor.Views.UserControlViews
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        public Register()
        {
            InitializeComponent();
        }
        private EIPProductionIndexUsersRegisterService eIPProductionIndexUsersRegisterService = new EIPProductionIndexUsersRegisterService();
        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateLoginParameters())
            {
                return;
            }
            EIPProductionIndexUsers eIPProductionIndexUsers = new EIPProductionIndexUsers()
            {
                Email = this.emailTextbox.Text,
                EmployeeId = this.employeeIDTextbox.Text,
                UserName = this.userNameTextbox.Text,
                Password = this.passwordTextbox.Text,
                ClearTextPassword = this.passwordTextbox.Text,
                CreatedDate = DateTime.Now,
                Status = Model.Widget.Status.Inactive,
                StatusName = Model.Widget.Status.Inactive.ToString()
            };

            eIPProductionIndexUsers.Password = UserEncryptionService.Encrypt(eIPProductionIndexUsers, eIPProductionIndexUsers.ClearTextPassword);
            var result = await eIPProductionIndexUsersRegisterService.Register(eIPProductionIndexUsers).ConfigureAwait(false);
            if (result != null)
            {
                Messenger.Default.Send("创建成功，请通知IT与流程部进行账号赋权以及账户激活", "SendMessageToMainWin");
                return;
            }
            Messenger.Default.Send("创建失败", "SendMessageToMainWin");
        }
        private bool ValidateLoginParameters()
        {
            if (this.passwordTextbox.Text.Equals(this.confirmPasswordTextbox.Text) && !String.IsNullOrWhiteSpace(this.passwordTextbox.Text))
            {
                Messenger.Default.Send("密码不匹配", "SendMessageToMainWin");
                return false;
            }
            if (String.IsNullOrWhiteSpace(this.userNameTextbox.Text) || String.IsNullOrWhiteSpace(this.employeeIDTextbox.Text) || String.IsNullOrWhiteSpace(this.emailTextbox.Text))
            {
                Messenger.Default.Send("用户名,密码,邮箱编号必填", "SendMessageToMainWin");
                return false;
            }
            return true;
        }
    }
}
