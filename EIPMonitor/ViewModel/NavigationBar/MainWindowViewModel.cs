using EIPMonitor.DomainServices.MasterData;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Views.PowerMeter;
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
using System.Windows.Controls;

namespace EIPMonitor.ViewModel.NavigationBar
{
    public class MainWindowViewModel: ViewModelBase
    {
        private IEIP_PRO_GlobalParamConfigureService eIP_PRO_GlobalParamConfigureService;
        public MainWindowViewModel()
        {
            NavigationItems = new ObservableCollection<NavigationItem>(new[]
            {
                new NavigationItem(
                    "Welcome",
                    typeof(Welcome)

                ),
                new NavigationItem("WODataImport",typeof(WODataImport))
            }) ;
            eIP_PRO_GlobalParamConfigureService = IocKernel.Get<IEIP_PRO_GlobalParamConfigureService>();
            //var items = GenerateNavigationItems(snackbarMessageQueue)?.OrderBy(i => i.Name)??null;
            //if (items != null)
            //    foreach (var item in items)
            //    {
            //        NavigationItems.Add(item);
            //    }

        }

        private NavigationItem? _selectedItem;
        private int _selectedIndex;
        private bool _controlsEnabled = true;
        private IUserStamp _user;
        private string _name;
        private int intializeTotalSteps = 8;
        public IUserStamp User { get => _user; set { _user = value; this.Name = _user.UserName; } }
        
        public String Name { get => _name; set => SetProperty(ref _name, value); }
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
            Messenger.Default.Send($"1/{intializeTotalSteps}:获取登录信息", "SendMessageToLoginWin");
            this.User = IocKernel.Get<IUserStamp>();
            Messenger.Default.Send($"2/{intializeTotalSteps}:{this.Name},欢迎使用EIP监控平台。", "SendMessageToLoginWin");
            if (!this.User.IsAvailable())
            {
                ApplicationShutdown();
                return;
            }

            Messenger.Default.Send($"3/{intializeTotalSteps}:验证版本信息", "SendMessageToLoginWin");
            //VeriyVersion().Wait();
            Messenger.Default.Send($"4/{intializeTotalSteps}:版本正确", "SendMessageToLoginWin");

            Messenger.Default.Send($"5/{intializeTotalSteps}:配置数据库信息", "SendMessageToLoginWin");
            //InitializeTheGlobalStaticParameter().Wait();
            Messenger.Default.Send($"6/{intializeTotalSteps}:配置数据库信息", "SendMessageToLoginWin");
            //Messenger.Default.Send(String.Empty, "ApplicationShutdown");
        }
        public void ApplicationShutdown()
        {
            Messenger.Default.Send(String.Empty, "MainWindowShutdown");
        }
        public Version CurrentVersion { get => ApplicationDeployment.IsNetworkDeployed ? ApplicationDeployment.CurrentDeployment.CurrentVersion : Assembly.GetExecutingAssembly().GetName().Version; }
        private async Task VeriyVersion()
        {
            try
            {
                var config = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.Software_Version).ConfigureAwait(true);

                var verifiedVersion = new Version(config.Parameter);
                if (verifiedVersion.Major != CurrentVersion.Major || verifiedVersion.Minor != CurrentVersion.Minor)
                {
                    Messenger.Default.Send("此软件版本不匹配最新的软件版本，请联系流程与IT管理部丁龙飞(00074729)获取最新版本", "SendMessageToLoginWin");
                    return;
                }
            }
            catch (Exception e)
            {
                Messenger.Default.Send("未能获取到版本信息", "SendMessageToLoginWin");
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

        private static IEnumerable<NavigationItem> GenerateNavigationItems(ISnackbarMessageQueue snackbarMessageQueue)
        {
            if (snackbarMessageQueue is null)
                throw new ArgumentNullException(nameof(snackbarMessageQueue));
            return default;
        }
    }
}
