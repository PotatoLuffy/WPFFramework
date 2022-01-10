using EIPMonitor.CustomUserControlRepository;
using EIPMonitor.DomainServices.MasterData;
using EIPMonitor.DomainServices.SecurityServices.FunctionServices;
using EIPMonitor.DomainServices.SecurityServices.UserRolesServices;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.SecurityModel;
using EIPMonitor.Views.Automation;
using EIPMonitor.Views.PowerMeter;
using EIPMonitor.Views.UserControlViews;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace EIPMonitor.ViewModel.NavigationBar
{
    public class MainWindowViewModel: ViewModelBase
    {
        private IEIP_PRO_GlobalParamConfigureService eIP_PRO_GlobalParamConfigureService;
        private EIP_MONITOR_PAGESearchService eIP_MONITOR_PAGESearchService = new EIP_MONITOR_PAGESearchService();
        private EIP_MONITOR_ROLE_USERSearchService eIP_MONITOR_ROLE_USERSearchService = new EIP_MONITOR_ROLE_USERSearchService();
        private static IReadOnlyDictionary<String, Type> UserControlMapper = null;
        public MainWindowViewModel()
        {
            UserControlMapper = new Dictionary<String, Type>()
        {
            { "WODataImport", typeof(WODataImport)},
            { "ProductionIndexQueryUserControl",  typeof(ProductionIndexQueryUserControl) },
            { "EIPParameterSetUserControl",  typeof(EIPParameterSetUserControl)},
            { "EIPMiddleWareDataSimulation",  typeof(EIPMiddleWareDataSimulation)},
            { "Welcome", typeof(Welcome)},
            { "WODataImportAutomation",  typeof(WODataImportAutomation)},
            { "ProductionIndexQueryUserControlAutomation",  typeof(ProductionIndexQueryAutomation)},
            { "EIP_MO_CheckWin",  typeof(EIP_MO_CheckWin)},
            { "EIP_MO_CheckWinAutomation",  typeof(EIP_MO_CheckWinAutomation)},
            { "RolePermissionConfiguration", typeof(RolePermissionConfiguration)},
            { "PageConfiguration",  typeof(PageConfiguration) },
            { "PromoteTheScoreAutomation",  typeof(PromoteTheScoreAutomation) },
            { "PromoteTheScorePowerMeter",  typeof(PromoteTheScorePowerMeter) },
            { "Register",  typeof(Register) },
        };
            //NavigationItems = new ObservableCollection<NavigationItem>(new[]
            //{
            //    new NavigationItem("欢迎",typeof(Welcome)),
            //    new NavigationItem("[电能表]工单同步",typeof(WODataImport)),
            //    new NavigationItem("[电能表]工单查询",typeof(ProductionIndexQueryUserControl)),
            //    new NavigationItem("[电能表]工单提交",typeof(EIP_MO_CheckWin)),
            //    new NavigationItem("[电能表]模拟提分",typeof(PromoteTheScorePowerMeter)),
            //    new NavigationItem("[电能表]模拟数据",typeof(EIPMiddleWareDataSimulation)),
            //    new NavigationItem("[电能表]参数设置",typeof(EIPParameterSetUserControl)),
            //    new NavigationItem("[自动化]工单同步",typeof(WODataImportAutomation)),
            //    new NavigationItem("[自动化]工单查询",typeof(ProductionIndexQueryAutomation)),
            //    new NavigationItem("[自动化]模拟提分",typeof(PromoteTheScoreAutomation)),
            //}) ;
            eIP_PRO_GlobalParamConfigureService = IocKernel.Get<IEIP_PRO_GlobalParamConfigureService>();
            InitializeBasicInformation();
        }

        private NavigationItem? _selectedItem;
        private int _selectedIndex;
        private bool _controlsEnabled = true;
        private IUserStamp _user;
        private string _name;
        private int intializeTotalSteps = 8;
        private string _msg;
        private bool _showMsg = false;
        public string Msg { get => _msg; set { SetProperty(ref _msg, value); ShowMsg = String.IsNullOrEmpty(Msg); } }
        public bool ShowMsg { get => _showMsg; set => SetProperty(ref _showMsg, value); }
        public IUserStamp User { get => _user; set { _user = value; this.Name = _user.UserName; } }
        
        public String Name { get => _name; set => SetProperty(ref _name, value); }
        private ObservableCollection<NavigationItem> _navigationItems;
        public ObservableCollection<NavigationItem> NavigationItems { get; set; }

        public NavigationItem? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }
        public bool ControlsEnabled
        {
            get => _controlsEnabled;
            set => SetProperty(ref _controlsEnabled, value);
        }
        public void InitializeBasicInformation()
        {
            Messenger.Default.Send($"1/{intializeTotalSteps}:获取登录信息", "SendMessageToMainWin");
            this.User = IocKernel.Get<IUserStamp>();
            Messenger.Default.Send($"2/{intializeTotalSteps}:{this.Name},欢迎使用EIP监控平台。", "SendMessageToMainWin");
            if (!this.User.IsAvailable())
            {
                ApplicationShutdown();
                return;
            }

            Messenger.Default.Send($"3/{intializeTotalSteps}:验证版本信息", "SendMessageToMainWin");
            var verifyTask = VeriyVersion();
            verifyTask.Wait();
            if (verifyTask.Result)
                Messenger.Default.Send($"4/{intializeTotalSteps}:版本正确", "SendMessageToMainWin");
            else {
                MessageBox.Show("此软件版本不匹配最新的软件版本，请联系流程与IT管理部丁龙飞(00074729)获取最新版本");
                ApplicationShutdown();
                return;
            }
            Messenger.Default.Send($"5/{intializeTotalSteps}:配置数据库信息", "SendMessageToMainWin");
            InitializeTheGlobalStaticParameter().Wait();
            Messenger.Default.Send($"6/{intializeTotalSteps}:配置数据库信息", "SendMessageToMainWin");
            Messenger.Default.Send($"7/{intializeTotalSteps}:加载授权页面", "SendMessageToMainWin");
            var pageTask = ReloadPages();
            pageTask.Wait();
            NavigationItems = pageTask.Result;
            Messenger.Default.Send($"7/{intializeTotalSteps}:授权页面加载完成", "SendMessageToMainWin");
            Messenger.Default.Send($"8/{intializeTotalSteps}:验证管理员权限", "SendMessageToMainWin");
            VerifyAdmin().Wait();
            Messenger.Default.Send($"8/{intializeTotalSteps}:验证管理员权限结束", "SendMessageToMainWin");
        }
        public void ApplicationShutdown()
        {
            Messenger.Default.Send(String.Empty, "MainWindowShutdown");
        }
        public Version CurrentVersion { get => ApplicationDeployment.IsNetworkDeployed ? ApplicationDeployment.CurrentDeployment.CurrentVersion : Assembly.GetExecutingAssembly().GetName().Version; }
        private async Task<bool> VeriyVersion()
        {
            try
            {
                var config = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.Software_Version).ConfigureAwait(true);

                var verifiedVersion = new Version(config.Parameter);
                if (verifiedVersion.Major != CurrentVersion.Major || verifiedVersion.Minor != CurrentVersion.Minor)
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Messenger.Default.Send("未能获取到版本信息", "SendMessageToLoginWin");
                return false;
            }
        }
        private async Task InitializeTheGlobalStaticParameter()
        {
            try
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
                    Messenger.Default.Send("未配置Current_SGCCConnection，请联系流程与IT部配置Current_SGCCConnection", "SendMessageToLoginWin");
                    return;
                }
            }
            catch (Exception e)
            {
                Messenger.Default.Send("配置未获取完全", "SendMessageToLoginWin");
            }
        }
        private async Task<ObservableCollection<NavigationItem>> ReloadPages()
        {
            ObservableCollection<NavigationItem> navigationItems = new ObservableCollection<NavigationItem>();
            var result = await eIP_MONITOR_PAGESearchService.GetFunctions(IocKernel.Get<IUserStamp>()).ConfigureAwait(true);
            if (result == null || result.Count <= 0)
            {
                navigationItems.Add(new NavigationItem("欢迎", typeof(Welcome)));
                return navigationItems;
            }
            foreach (var page in result.OrderBy(o=>o.OrderWeight))
            {
                navigationItems.Add(new NavigationItem(page.PAGE_NAME, UserControlMapper[page.PAGE_FUNCTION_NAME]));
            }
            return navigationItems;
        }
        private async Task VerifyAdmin()
        {
            var userRolesTask = eIP_MONITOR_ROLE_USERSearchService.GetSpecificUserRoles(new EIP_MONITOR_ROLE_USER() { EMPLOYEEID = IocKernel.Get<IUserStamp>().EmployeeId });
            userRolesTask.Wait();
            LocalConstant.IsAdmin = userRolesTask.Result.Exists(w => w.ROLE_NAME == "管理员");
        }
        private static IEnumerable<NavigationItem> GenerateNavigationItems(ISnackbarMessageQueue snackbarMessageQueue)
        {
            if (snackbarMessageQueue is null)
                throw new ArgumentNullException(nameof(snackbarMessageQueue));
            return default;
        }
    }
}
