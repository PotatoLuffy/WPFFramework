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

        public Login()
        {
            InitializeComponent();
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
    }
}
