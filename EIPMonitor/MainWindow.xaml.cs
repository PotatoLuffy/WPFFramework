using EIPMonitor.CustomUserControlRepository;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.ViewModel.MessagesViewModels;
using EIPMonitor.ViewModel.NavigationBar;
using EIPMonitor.Views.UserWindowViews;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Login login;
        private Boolean ifNewInstance;
        private MainWindowViewModel mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            Task.Factory.StartNew(() => Thread.Sleep(2500)).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue?.Enqueue("Welcome to EIP Monitor");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            mainWindowViewModel = new MainWindowViewModel(MainSnackbar.MessageQueue!);
            DataContext = mainWindowViewModel;
            this.Visibility = Visibility.Hidden;
            Mutex myMutex = new Mutex(true, "EIPMonitor", out ifNewInstance);
            if (!ifNewInstance)
            {
                this.Close();
                Environment.Exit(0);
                return;
            }
            login = new Login();
            login.Show();
            login.Closed += LoginWinClosed_EventHandler;
        }
        private void LoginWinClosed_EventHandler(object sender, EventArgs e)
        {
            mainWindowViewModel.User = IocKernel.Get<IUserStamp>();
            if (!ifNewInstance || !mainWindowViewModel.User.IsAvailable())
            {
                Environment.Exit(0);
                return;
            }
            this.Visibility = Visibility.Visible;
        //    LoginWindow.CurrentUser = this.loginWin.AuthorizedUser;
        //    LoginWindow.UserStamp = new Model.UserStamp() { UserName = LoginWindow.CurrentUser.UserName, EmployeeId = LoginWindow.CurrentUser.EmployeeId };
        //    var config = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.Software_Version).ConfigureAwait(true);
        //    verifiedVersion = new Version(config.Parameter);
        //    if (verifiedVersion.Major != CurrentVersion.Major || verifiedVersion.Minor != CurrentVersion.Minor)
        //    {
        //        MessageBox.Show("此软件版本不匹配最新的软件版本，请联系流程与IT管理部丁龙飞(00074729)获取最新版本");
        //        loginWin.Close();
        //        this.Close();
        //        return;
        //    }
        //    InitializeTheGlobalStaticParameter().Wait();
        //    var oracleDbConnection = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.Current_SGCCConnection).ConfigureAwait(true);
        //    if (oracleDbConnection == null || String.IsNullOrWhiteSpace(oracleDbConnection.Parameter))
        //    {
        //        MessageBox.Show("未配置Current_SGCCConnection，请联系流程与IT部配置Current_SGCCConnection");
        //        loginWin.Close();
        //        this.Close();
        //        return;
        //    }
        //    LocalConstant.OracleCurrentConnectionStringBuilder = JsonConvert.DeserializeObject<OracleConnectionStringBuilder>(oracleDbConnection.Parameter);
        //    //this.OracleConnectionLabel.Content = "中间库:测试";
        //    //this.MesConnectionLabel.Content = "MES库:测试";
        //    if (LocalConstant.oracleConnectionStringBuilder.DataSource == LocalConstant.OracleCurrentConnectionStringBuilder.DataSource)
        //    {
        //        LocalConstant.CurrentMESDBConnection = LocalConstant.MESStandardDB;
        //        this.MesConnectionLabel.Content = "EIP监控平台";
        //        //this.OracleConnectionLabel.Content = "中间库:正式";
        //        //this.MesConnectionLabel.Content = "MES库:正式";
        //        //MessageBox.Show("由于当前为国网正式数据库，所以MES数据库切换为正式");
        //    }
        //    else
        //    {
        //        LocalConstant.CurrentMESDBConnection = LocalConstant.MESTestDB;
        //        //this.OracleConnectionLabel.Content = "中间库:测试";
        //        //this.MesConnectionLabel.Content = "MES库:测试";
        //        //MessageBox.Show("由于当前为国网测试数据库，所以MES数据库切换为测试");
        //    }
        //    if (!this.IsClosed)
        //        this.Visibility = Visibility.Visible;
        //    if (loginWin.AuthorizedUser == null)
        //    {
        //        loginWin.Close();
        //        this.Close();
        //        return;
        //    }
        //    loginWin.Close();

            //    var userRolesTask = eIP_MONITOR_ROLE_USERSearchService.GetSpecificUserRoles(new EIP_MONITOR_ROLE_USER() { EMPLOYEEID = this.loginWin.AuthorizedUser.EmployeeId });
            //    userRolesTask.Wait();
            //    eIP_MONITOR_ROLE_USERs = userRolesTask.Result;
            //    LocalConstant.IsAdmin = eIP_MONITOR_ROLE_USERs.Exists(w => w.ROLE_NAME == "管理员");
            //    this.userNameDip.Content = loginWin.AuthorizedUser.UserName;
            //    UserControlMapper = new Dictionary<String, UserControl>()
            //{
            //    { "WODataImport", new WODataImport()},
            //    { "ProductionIndexQueryUserControl", new ProductionIndexQueryUserControl() },
            //    { "EIPParameterSetUserControl", new EIPParameterSetUserControl()},
            //    { "EIPMiddleWareDataSimulation", new EIPMiddleWareDataSimulation()},
            //    { "Welcome",new Welcome()},
            //    { "WODataImportAutomation", new WODataImportAutomation()},
            //    { "ProductionIndexQueryUserControlAutomation", new ProductionIndexQueryAutomation()},
            //    { "EIP_MO_CheckWin", new EIP_MO_CheckWin()},
            //    { "EIP_MO_CheckWinAutomation", new EIP_MO_CheckWinAutomation()},
            //    { "RolePermissionConfiguration",new RolePermissionConfiguration()},
            //    { "PageConfiguration", new PageConfiguration() },
            //    { "PromoteTheScoreAutomation", new PromoteTheScoreAutomation() },
            //    { "PromoteTheScorePowerMeter", new PromoteTheScorePowerMeter() },
            //};
            //    LocalConstant.SetUpConnection(LocalConstant.OracleCurrentConnectionStringBuilder.ToString());
            //    LocalConstant.SetUpLogger();
            //    ReloadPages().Wait();
            //    //var permissionInx =  this.NavigationMenuListBox.Items.IndexOf("权限配置");
            //    //var list = this.NavigationMenuListBox.ItemsSource as List<String>;
            //    //list.RemoveAt(permissionInx);
            //    //this.NavigationMenuListBox.ItemsSource = list;
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
