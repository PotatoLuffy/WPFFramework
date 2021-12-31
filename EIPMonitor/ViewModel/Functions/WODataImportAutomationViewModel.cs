using EIPMonitor.Databse;
using EIPMonitor.Databse.Generic;
using EIPMonitor.DomainServices.MasterData;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.MasterData;
using EIPMonitor.Model.SGCC_Master;
using EIPMonitor.Model.Widget;
using GalaSoft.MvvmLight.Messaging;
using Infrastructure.Standard.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.ViewModel.Functions
{
    public class WODataImportAutomationViewModel : ViewModelBase
    {
        private String eIP_CAL_MO_SCORE_CurrentVersion = null;
        private readonly IReadOnlyDictionary<String, String> btnProcedureMapper = new Dictionary<String, String>()
                {
                    { "HighVoltageInsulationTestButton", "exec [dbo].[CL_SGCCPRD_ACVOLTEST_SAVE] @IPONO "  },
                    { "IntrinsicErrorButton", " exec [dbo].CL_SGCCPRD_EM_EXP_PRO_BASICER_SAVE @IPONO "},
                    { "IntrinsicErrorDetailButton", " exec [dbo].[CL_SGCCPRD_EM_EXP_PRO_BASICER_DETAIL_SAVE] @IPONO "},
                    { "DailyTimingErrorButton", "exec [dbo].[CL_SGCCPRD_EM_EXP_PRO_DAYTIMEER_SAVE] @IPONO "},
                    { "ParameterSettingButton","exec [dbo].[CL_SGCCPRD_EM_EXP_PRO_PARAMSET_SAVE] @IPONO "},
                    { "BatteryCurrentButton","exec [dbo].[CL_SGCCPRD_PP_TEST_BATCUR_SAVE] @IPONO "},
                    { "FuncionTestButton","exec [dbo].[CL_SGCCPRD_PP_TEST_FCT_SAVE] @IPONO "},
                    { "AOITestButton","exec [dbo].[CL_SGCCPRD_PP_TEST_PCBAOI_SAVE] @IPONO "},
                    { "AutomationButton", "exec [dbo].[CL_SGCCPRD_EXP_PRO_ACAVALUE_AUTO_SAVE] @IPONO " },
                    { "AutomationConnectionChannelButton"," exec [dbo].CL_SGCCPRD_EXP_PRO_CCTEST_AUTO_SAVE @IPONO "},
                    { "AutomationParameterButton", "exec [dbo].CL_SGCCPRD_EXP_PRO_PARAMSET_AUTO_SAVE @IPONO "},
                    { "ProductionStoreButton", "exec [dbo].[CL_SGCCPRD_PRODUCTSTORAGE_SAVE] @IPONO "  }
                };
        private string _HighVoltageInsulationTestTextBlock;
        private string _IntrinsicErrorTextBlock;
        private string _IntrinsicErrorDetailTextBlock;
        private string _DailyTimingErrorTextBlock;
        private string _ParameterSettingTextBlock;
        private string _BatteryCurrentTextBlock;
        private string _FuncionTestTextBlock;
        private string _AOITestTextBlock;
        private string _AutomationTextBlock;
        private string _AutomationConnectionChannelTextBlock;
        private string _AutomationParameterTextBlock;
        private string _ProductionStoreTextBlock;
        private string _moName;

        private string _MOQtyRequirement;
        private string _materialCodeTextbox;
        private string _materialNameTextbox;

        public String HighVoltageInsulationTestTextBlock { get => _HighVoltageInsulationTestTextBlock; set => SetProperty(ref _HighVoltageInsulationTestTextBlock, value); }
        public String IntrinsicErrorTextBlock { get => _IntrinsicErrorTextBlock; set => SetProperty(ref _IntrinsicErrorTextBlock, value); }
        public String IntrinsicErrorDetailTextBlock { get => _IntrinsicErrorDetailTextBlock; set => SetProperty(ref _IntrinsicErrorDetailTextBlock, value); }
        public String DailyTimingErrorTextBlock { get => _DailyTimingErrorTextBlock; set => SetProperty(ref _DailyTimingErrorTextBlock, value); }
        public String ParameterSettingTextBlock { get => _ParameterSettingTextBlock; set => SetProperty(ref _ParameterSettingTextBlock, value); }
        public String BatteryCurrentTextBlock { get => _BatteryCurrentTextBlock; set => SetProperty(ref _BatteryCurrentTextBlock, value); }
        public String FuncionTestTextBlock { get => _FuncionTestTextBlock; set => SetProperty(ref _FuncionTestTextBlock, value); }
        public String AOITestTextBlock { get => _AOITestTextBlock; set => SetProperty(ref _AOITestTextBlock, value); }
        public String AutomationTextBlock { get => _AutomationTextBlock; set => SetProperty(ref _AutomationTextBlock, value); }
        public String AutomationConnectionChannelTextBlock { get => _AutomationConnectionChannelTextBlock; set => SetProperty(ref _AutomationConnectionChannelTextBlock, value); }
        public String AutomationParameterTextBlock { get => _AutomationParameterTextBlock; set => SetProperty(ref _AutomationParameterTextBlock, value); }
        public String ProductionStoreTextBlock { get => _ProductionStoreTextBlock; set => SetProperty(ref _ProductionStoreTextBlock, value); }

        public String MOQtyRequirement { get => _MOQtyRequirement; set => _MOQtyRequirement = value; }
        public String materialCodeTextbox { get => _materialCodeTextbox; set => _materialCodeTextbox = value; }
        public String materialNameTextbox { get => _materialNameTextbox; set => _materialNameTextbox = value; }
        public String moName { get => _moName; set => _moName = value; }
        private readonly IReadOnlyDictionary<String, Action<String>> btnCommentMapper;
        private readonly GenericExecutionService<ZGW_MO_T> genericExecutionService = new GenericExecutionService<ZGW_MO_T>();
        private IEIP_PRO_GlobalParamConfigureService eIP_PRO_GlobalParamConfigureService;
        private TimeSpan synchronous_black_range_begin_time;
        private TimeSpan synchronous_black_range_end_time;
        private TimeSpan synchronous_Black_Range_Begin_Break_Time_Start;
        private TimeSpan synchronous_Black_Range_Begin_Break_Time_End;
        private ZCL_SIMUL_DService zCL_SIMUL_DService = new ZCL_SIMUL_DService();
        private CRUDService cRUDService = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
        private List<ZCL_SIMUL_D> details;
        private List<MES_MO_TO_EIP_POOL> mES_MO_TO_EIP_POOLs;
        private readonly string woVerifySqlText = "select * from ZGW_MO_T where IPONO = :IPONO and rownum = 1";

        public List<MES_MO_TO_EIP_POOL> MES_MO_TO_EIP_POOLs { get => mES_MO_TO_EIP_POOLs; set => SetProperty(ref mES_MO_TO_EIP_POOLs, value); }
        public List<ZCL_SIMUL_D> Details { get => details; set => SetProperty(ref details, value); }
        public WODataImportAutomationViewModel()
        {
            eIP_PRO_GlobalParamConfigureService = IocKernel.Get<IEIP_PRO_GlobalParamConfigureService>();

            btnCommentMapper = new Dictionary<String, Action<String>>()
                                                                            {
                                                                                { "HighVoltageInsulationTestButton", (string msg)=> this.HighVoltageInsulationTestTextBlock = msg },
                                                                                { "IntrinsicErrorButton",(string msg)=> this.IntrinsicErrorTextBlock = msg },
                                                                                { "IntrinsicErrorDetailButton",(string msg)=> this.IntrinsicErrorDetailTextBlock = msg },
                                                                                { "DailyTimingErrorButton",(string msg)=> this.DailyTimingErrorTextBlock = msg },
                                                                                { "ParameterSettingButton",(string msg)=> this.ParameterSettingTextBlock = msg },
                                                                                { "BatteryCurrentButton",(string msg)=> this.BatteryCurrentTextBlock = msg },
                                                                                { "FuncionTestButton",(string msg)=> this.FuncionTestTextBlock = msg },
                                                                                { "AOITestButton",(string msg)=> this.AOITestTextBlock = msg },
                                                                                { "AutomationButton",(string msg)=> this.AutomationTextBlock = msg },
                                                                                { "AutomationConnectionChannelButton",(string msg)=> this.AutomationConnectionChannelTextBlock = msg },
                                                                                { "AutomationParameterButton",(string msg)=> this.AutomationParameterTextBlock = msg },
                                                                                { "ProductionStoreButton",(string msg)=> this.ProductionStoreTextBlock = msg }
                                                                            };
        }
        private async Task InitializeGlobalParameter(IEIP_PRO_GlobalParamConfigureService eIP_PRO_GlobalParamConfigureService)
        {
            var begin = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(EIP_PRO_GlobalParameter.Synchronous_Black_Range_Begin_Time).ConfigureAwait(true);
            var end = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(EIP_PRO_GlobalParameter.Synchronous_Black_Range_End_Time).ConfigureAwait(true);
            var breakBeginTask = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(EIP_PRO_GlobalParameter.Synchronous_Black_Range_Begin_Break_Time_Start).ConfigureAwait(true);
            var breakEndTask = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(EIP_PRO_GlobalParameter.Synchronous_Black_Range_Begin_Break_Time_End).ConfigureAwait(true);

            synchronous_black_range_begin_time = TimeSpan.Parse(begin.Parameter);
            synchronous_black_range_end_time = TimeSpan.Parse(end.Parameter);

            synchronous_Black_Range_Begin_Break_Time_Start = TimeSpan.Parse(breakBeginTask?.Parameter ?? "12:00");
            synchronous_Black_Range_Begin_Break_Time_End = TimeSpan.Parse(breakEndTask?.Parameter ?? "13:30");
        }
        public async Task TriggerTheButtonRelativeAction(string btnName)
        {
            DateTime now = DateTime.Now;
            if ((now.TimeOfDay >= synchronous_black_range_begin_time && now.TimeOfDay <= synchronous_Black_Range_Begin_Break_Time_Start)
                || (now.TimeOfDay >= synchronous_Black_Range_Begin_Break_Time_End && now.TimeOfDay <= synchronous_black_range_end_time)
                || LocalConstant.IsAdmin
                )
            {
                Messenger.Default.Send("上班时间禁止同步。", "SendMessageToMainWin");
                return;
            }

            try
            {
                Messenger.Default.Send("开始验证单号。", "SendMessageToMainWin");
                var verifyResult = await VerifyTheMOName().ConfigureAwait(true);
                if (verifyResult == null)
                {
                    Messenger.Default.Send("生产工单号不存在于ZGW_MO_T。", "SendMessageToMainWin");
                    return;
                }
                Messenger.Default.Send("工单号验证成功。", "SendMessageToMainWin");
                btnCommentMapper[btnName]("开始同步,请稍后。");

                SynchronousProductionData(btnName, verifyResult);
            }
            catch (Exception e1)
            {
                Messenger.Default.Send(e1.Message, "SendMessageToMainWin");
                LocalConstant.Logger.Debug("WODataImport_ButtonClick", e1);
            }
        }
        public async Task<ZGW_MO_T> VerifyTheMOName()
        {
            if (!moName.StartsWith("6") && !LocalConstant.IsAdmin)
            {
                Messenger.Default.Send("请输入以6开头的电能表工单。", "SendMessageToMainWin");
                return null;
            }
            ZGW_MO_T workOrder = new ZGW_MO_T() { IPONO = moName };
            var verifyResult = await genericExecutionService.SGCCQueryAsync(woVerifySqlText, workOrder).ConfigureAwait(true);

            if (verifyResult == null)
            {
                Messenger.Default.Send("该工单不需要同步。", "SendMessageToMainWin");
                return null;
            }
            return verifyResult;
        }
        private async void SynchronousProductionData(string buttonName, ZGW_MO_T veririedMo)
        {

            var procedures = this.btnProcedureMapper[buttonName];
            var commentBlock = this.btnCommentMapper[buttonName];

            try
            {
                var result = await genericExecutionService.TestExecuteAsync(procedures, veririedMo).ConfigureAwait(true);
                this.btnCommentMapper[buttonName]($"同步成功影响的数据为:{result}");
            }
            catch (Exception e)
            {
                LocalConstant.Logger.Debug($"WODataImport_Synchronous_{buttonName}", e);

                if (e.Message.IndexOf("完整性约束") >= 0)
                {
                    this.btnCommentMapper[buttonName]("同步失败, 违反EIP完整性约束。");
                    return;
                }
                this.btnCommentMapper[buttonName]("此项未同步成功或者已经同步过，请手动处理。");
            }
        }
        public async Task GetMOInformation()
        {
            try
            {
                InitializeGlobalParameter(eIP_PRO_GlobalParamConfigureService).Wait();
                var result = await VerifyTheMOName().ConfigureAwait(true);
                if (result == null) return;
                this.MOQtyRequirement = result.AMOUNT.ToString("###,###");
                this.materialCodeTextbox = result.MATERIALSCODE;
                this.materialNameTextbox = result.MATERIALSNAME;
                CleanAllTheCommentTextBlock();
            }
            catch (Exception e1)
            {
                LocalConstant.Logger.Debug("AppDomainUnhandledExceptionHandler", e1);
                this.materialNameTextbox = "该工单不是一个自动化工单";
            }
        }
        private void CleanAllTheCommentTextBlock()
        {
            foreach (var keyValuePair in btnCommentMapper)
            {
                keyValuePair.Value(String.Empty);
            }
        }
        public async Task CalculateTheScore()
        {
            if (String.IsNullOrWhiteSpace(eIP_CAL_MO_SCORE_CurrentVersion))
            {
                var config = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.EIP_CAL_MO_SCORE_CurrentVersion).ConfigureAwait(true);
                eIP_CAL_MO_SCORE_CurrentVersion = config.Parameter;
            }
            EIP_CAL_MO_SCOREParameter eIP_CAL_MO_SCOREParameter = new EIP_CAL_MO_SCOREParameter() { pI_Method = PI_Method.Calculate_Specific_Mo_Score, PI_Mo = moName };
            var calculatedScore = await cRUDService.Create<EIP_CAL_MO_SCOREParameter>($"call {eIP_CAL_MO_SCORE_CurrentVersion}(:pI_Method,:PI_Mo)", eIP_CAL_MO_SCOREParameter).ConfigureAwait(true);

            details = await zCL_SIMUL_DService.GetEntries(eIP_CAL_MO_SCOREParameter.PI_Mo, eIP_CAL_MO_SCOREParameter.PI_Mo, null, '5').ConfigureAwait(true);

            if (details == null || details.Count == 0)
            {
                Messenger.Default.Send("未找到任何数据。", "SendMessageToMainWin");
                return;
            }
            mES_MO_TO_EIP_POOLs = details.Aggregate(IocKernel.Get<IUserStamp>());
        }
    }
}
