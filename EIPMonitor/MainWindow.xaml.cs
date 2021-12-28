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
        private Login login;
        private Boolean ifNewInstance;
        private MainWindowViewModel mainWindowViewModel;
        private IEIP_PRO_GlobalParamConfigureService eIP_PRO_GlobalParamConfigureService;
        public MainWindow()
        {
            InitializeComponent();
            Task.Factory.StartNew(() => Thread.Sleep(2500)).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue?.Enqueue("Welcome to EIP Monitor");
            }, TaskScheduler.FromCurrentSynchronizationContext());
            eIP_PRO_GlobalParamConfigureService = IocKernel.Get<IEIP_PRO_GlobalParamConfigureService>();
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
            this.Visibility = Visibility.Visible;
            MainSnackbar.MessageQueue?.Enqueue("获取登录信息");
            mainWindowViewModel.User = IocKernel.Get<IUserStamp>();
            MainSnackbar.MessageQueue?.Enqueue($"{mainWindowViewModel.Name},欢迎使用EIP监控平台。");

            if (!ifNewInstance || !mainWindowViewModel.User.IsAvailable())
            {
                Environment.Exit(0);
                return;
            }


            MainSnackbar.MessageQueue?.Enqueue("验证版本信息");
            VeriyVersion().Wait();
            MainSnackbar.MessageQueue?.Enqueue("版本正确");

            MainSnackbar.MessageQueue?.Enqueue("配置数据库信息");
            InitializeTheGlobalStaticParameter().Wait();
            MainSnackbar.MessageQueue?.Enqueue("配置数据库信息成功");

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
        public Version CurrentVersion { get => ApplicationDeployment.IsNetworkDeployed ? ApplicationDeployment.CurrentDeployment.CurrentVersion : Assembly.GetExecutingAssembly().GetName().Version; }
        private async Task VeriyVersion()
        {
            var config = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.Software_Version).ConfigureAwait(true);
            
          var verifiedVersion = new Version(config.Parameter);
            if (verifiedVersion.Major != CurrentVersion.Major || verifiedVersion.Minor != CurrentVersion.Minor)
            {
                MainSnackbar.MessageQueue?.Enqueue("此软件版本不匹配最新的软件版本，请联系流程与IT管理部丁龙飞(00074729)获取最新版本");
                Thread.Sleep(5000);
                login.Close();
                this.Close();
                return;
            }
        }
        private async Task InitializeTheGlobalStaticParameter()
        {
            var standardOracleDb = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.SGCCPROD).ConfigureAwait(true);
            var testOracleDb = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.SGCCQAS).ConfigureAwait(true);
            var mesTestDb = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.MES_TestDb).ConfigureAwait(true);
            var mesStandardDb = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.MESStandardDB).ConfigureAwait(true);
            var currentOracleDb = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.Current_SGCCConnection).ConfigureAwait(true);

            LocalConstant.oracleConnectionStringBuilder = standardOracleDb != null && !String.IsNullOrEmpty(standardOracleDb.Parameter) ? JsonConvert.DeserializeObject<OracleConnectionStringBuilder>(standardOracleDb.Parameter) : LocalConstant.oracleConnectionStringBuilder;
            LocalConstant.oracleConnectionStringBuilderTest = testOracleDb != null && !String.IsNullOrEmpty(testOracleDb.Parameter) ? JsonConvert.DeserializeObject<OracleConnectionStringBuilder>(testOracleDb.Parameter) : LocalConstant.oracleConnectionStringBuilderTest;
            LocalConstant.MESTestDB = mesTestDb != null && !String.IsNullOrEmpty(mesTestDb.Parameter) ? JsonConvert.DeserializeObject<SqlConnectionStringBuilder>(mesTestDb.Parameter) : LocalConstant.MESTestDB;
            LocalConstant.MESStandardDB = mesStandardDb != null && !String.IsNullOrEmpty(mesStandardDb.Parameter) ? JsonConvert.DeserializeObject<SqlConnectionStringBuilder>(mesStandardDb.Parameter) : LocalConstant.MESStandardDB;
            LocalConstant.OracleCurrentConnectionStringBuilder = JsonConvert.DeserializeObject<OracleConnectionStringBuilder>(currentOracleDb.Parameter);
            if (LocalConstant.OracleCurrentConnectionStringBuilder == null || String.IsNullOrWhiteSpace(LocalConstant.OracleCurrentConnectionStringBuilder.ToString()))
            {
                MainSnackbar.MessageQueue?.Enqueue("未配置Current_SGCCConnection，请联系流程与IT部配置Current_SGCCConnection");
                Thread.Sleep(5000);
                login.Close();
                this.Close();
                return;
            }
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
