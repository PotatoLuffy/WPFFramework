using EIPMonitor.Databse;
using EIPMonitor.DomainServices.MasterData;
using EIPMonitor.DomainServices.OracleProcedureService;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model.MasterData;
using EIPMonitor.Model.SGCC_Master;
using EIPMonitor.Model.Widget;
using GalaSoft.MvvmLight.Messaging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.ViewModel.Functions
{
    public class EIPMiddleWareDataSimulationViewModel : ViewModelBase
    {
        private Dictionary<String, String> buttonProcedureMapper = new Dictionary<string, string>()
        {
            { "PCBAOIBtn","eip_aoi_data"},
            { "FCTBtn","eip_fct_data"},
            { "BatteryCurrrentBtn","eip_batcur_data"},
            { "AgingBtn","eip_aging_data"},
            { "ReflowBtn","eip_reflow_data"},
            { "WaveBtn","eip_wave_data"},
            { "HighVoltageBtn","eip_voltest_data"},
            { "IntrinsicErrorBtn","eip_basic_data"},
            { "IntrinsicErrorDetailBtn","eip_daytime_data"},
            { "DayTimingBtn","eip_basic_details"},
        };
        private DateTime? pCBAOIDatePicker;
        private DateTime? fCTDatePicker;
        private DateTime? batteryCurrrentDatePicker;
        private DateTime? agingDatePicker;
        private DateTime? reflowDatePicker;
        private DateTime? waveDatePicker;
        private DateTime? highVoltageDatePicker;
        private DateTime? intrinsicErrorDatePicker;
        private DateTime? intrinsicErrorDetailDatePicker;
        private DateTime? dayTimingDatePicker;
        public DateTime? PCBAOIDatePicker { get => pCBAOIDatePicker; set => SetProperty(ref pCBAOIDatePicker, value); }
        public DateTime? FCTDatePicker { get => fCTDatePicker; set => SetProperty(ref fCTDatePicker, value); }
        public DateTime? BatteryCurrrentDatePicker { get => batteryCurrrentDatePicker; set => SetProperty(ref batteryCurrrentDatePicker, value); }
        public DateTime? AgingDatePicker { get => agingDatePicker; set => SetProperty(ref agingDatePicker, value); }
        public DateTime? ReflowDatePicker { get => reflowDatePicker; set => SetProperty(ref reflowDatePicker, value); }
        public DateTime? WaveDatePicker { get => waveDatePicker; set => SetProperty(ref waveDatePicker, value); }
        public DateTime? HighVoltageDatePicker { get => highVoltageDatePicker; set => SetProperty(ref highVoltageDatePicker, value); }
        public DateTime? IntrinsicErrorDatePicker { get => intrinsicErrorDatePicker; set => SetProperty(ref intrinsicErrorDatePicker, value); }
        public DateTime? IntrinsicErrorDetailDatePicker { get => intrinsicErrorDetailDatePicker; set => SetProperty(ref intrinsicErrorDetailDatePicker, value); }
        public DateTime? DayTimingDatePicker { get => dayTimingDatePicker; set => SetProperty(ref dayTimingDatePicker, value); }

        public string pCBAOITextBlock;
        public string fCTTextBlock;
        public string batteryCurrrentTextBlock;
        public string agingTextBlock;
        public string reflowTextBlock;
        public string waveTextBlock;
        public string highVoltageTextBlock;
        public string intrinsicErrorTextBlock;
        public string intrinsicErrorDetailTextBlock;
        public string dayTimingTextBlock;

        public string PCBAOITextBlock { get => pCBAOITextBlock; set => SetProperty(ref pCBAOITextBlock, value); }
        public string FCTTextBlock { get => fCTTextBlock; set => SetProperty(ref fCTTextBlock, value); }
        public string BatteryCurrrentTextBlock { get => batteryCurrrentTextBlock; set => SetProperty(ref batteryCurrrentTextBlock, value); }
        public string AgingTextBlock { get => agingTextBlock; set => SetProperty(ref agingTextBlock, value); }
        public string ReflowTextBlock { get => reflowTextBlock; set => SetProperty(ref reflowTextBlock, value); }
        public string WaveTextBlock { get => waveTextBlock; set => SetProperty(ref waveTextBlock, value); }
        public string HighVoltageTextBlock { get => highVoltageTextBlock; set => SetProperty(ref highVoltageTextBlock, value); }
        public string IntrinsicErrorTextBlock { get => intrinsicErrorTextBlock; set => SetProperty(ref intrinsicErrorTextBlock, value); }
        public string IntrinsicErrorDetailTextBlock { get => intrinsicErrorDetailTextBlock; set => SetProperty(ref intrinsicErrorDetailTextBlock, value); }
        public string DayTimingTextBlock { get => dayTimingTextBlock; set => SetProperty(ref dayTimingTextBlock, value); }

        private string workOrderTextbox;
        public string WorkOrderTextbox { get => workOrderTextbox; set => SetProperty(ref workOrderTextbox, value); }
        private Dictionary<String, Action<DateTime?>> buttonDateMapper;
        private Dictionary<String, Func<DateTime?>> buttonDateGetMapper;
        private Dictionary<String, String> zkindMapper;
        private Dictionary<String, Action<String>> zkindActionMapper;
        private Dictionary<String, String> buttonTextBlockMapper;
        private string get_min_date;
        private readonly EIP_PRO_GlobalParamConfigureService eIP_PRO_GlobalParamConfigureService = new EIP_PRO_GlobalParamConfigureService();
        private readonly GenericExecutionService<EIP_MIN_DATA> genericExecutionService = new GenericExecutionService<EIP_MIN_DATA>();
        MOSearchService mOSearchService = new MOSearchService();
        private List<EIP_MIN_DATA> dateList;
        ZGW_MO_TYPECreateService zGW_MO_TYPECreateService = new ZGW_MO_TYPECreateService();
        MO mo;
        public EIPMiddleWareDataSimulationViewModel()
        {
            buttonDateMapper = new Dictionary<string, Action<DateTime?>>()
            {
                { "PCBAOIBtn", (date) =>this.PCBAOIDatePicker =date },
                { "FCTBtn",(date)  =>this.FCTDatePicker =date},
                { "BatteryCurrrentBtn",(date)  =>this.BatteryCurrrentDatePicker =date},
                { "AgingBtn",(date)  =>this.AgingDatePicker =date},
                { "ReflowBtn",(date)  =>this.ReflowDatePicker =date},
                { "WaveBtn",(date)  =>this.WaveDatePicker =date},
                { "HighVoltageBtn",(date)  =>this.HighVoltageDatePicker =date},
                { "IntrinsicErrorBtn",(date) =>this.IntrinsicErrorDatePicker =date},
                { "IntrinsicErrorDetailBtn",(date) =>this.IntrinsicErrorDetailDatePicker =date},
                { "DayTimingBtn",(date) =>this.DayTimingDatePicker =date},
            };
            buttonDateGetMapper = new Dictionary<string, Func<DateTime?>>()
            {
                { "PCBAOIBtn", () =>this.PCBAOIDatePicker },
                { "FCTBtn",()  =>this.FCTDatePicker},
                { "BatteryCurrrentBtn",()  =>this.BatteryCurrrentDatePicker},
                { "AgingBtn",()  =>this.AgingDatePicker },
                { "ReflowBtn",()  =>this.ReflowDatePicker },
                { "WaveBtn",()  =>this.WaveDatePicker },
                { "HighVoltageBtn",()  =>this.HighVoltageDatePicker },
                { "IntrinsicErrorBtn",() =>this.IntrinsicErrorDatePicker },
                { "IntrinsicErrorDetailBtn",() =>this.IntrinsicErrorDetailDatePicker },
                { "DayTimingBtn",() =>this.DayTimingDatePicker },
            };
            zkindMapper = new Dictionary<string, String>()
            {
                { "PCBAOIBtn","AOI"},
                { "FCTBtn","FCT"},
                { "BatteryCurrrentBtn","BAT"},
                { "AgingBtn","VOL"},
                { "ReflowBtn","AOI"},
                { "WaveBtn","FCT"},
                { "HighVoltageBtn","VOL"},
                { "IntrinsicErrorBtn","BAS"},
                { "IntrinsicErrorDetailBtn","BAS"},
                { "DayTimingBtn","DAY"},
            };
            zkindActionMapper = new Dictionary<string, Action<string>>()
            {
                { "AOI",(string button)=>{
                    var locakDatePicker = buttonDateMapper[button];  
                    locakDatePicker(dateList.Find(f=>f.ZKIND=="AOI").ZDATE);
                } },
                { "FCT",(string button)=>{ 
                    var locakDatePicker = buttonDateMapper[button]; 
                    locakDatePicker(dateList.Find(f=>f.ZKIND=="FCT").ZDATE); } },
                { "BAT",(string button)=>{  
                    var locakDatePicker = buttonDateMapper[button];  
                    locakDatePicker(dateList.Find(f=>f.ZKIND=="BAT").ZDATE); } },
                { "VOL",(string button)=>{ 
                    var locakDatePicker = buttonDateMapper[button];
                    if(button == "HighVoltageBtn")  locakDatePicker(dateList.Find(f=>f.ZKIND=="VOL").ZDATE);
                    else  locakDatePicker(dateList.Find(f=>f.ZKIND=="VOL").ZDATE.AddDays(1));
                    } },
                { "BAS",(string button)=>{ 
                    var locakDatePicker = buttonDateMapper[button];  
                    locakDatePicker(dateList.Find(f=>f.ZKIND=="BAS").ZDATE);
                    } },
                { "DAY",(string button)=>{ 
                    var locakDatePicker = buttonDateMapper[button];  
                    locakDatePicker(dateList.Find(f=>f.ZKIND=="BAS").ZDATE);
                } }
            };
            buttonTextBlockMapper = new Dictionary<string, String>()
            {
                { "PCBAOIBtn",this.PCBAOITextBlock},
                { "FCTBtn",this.FCTTextBlock},
                { "BatteryCurrrentBtn",this.BatteryCurrrentTextBlock},
                { "AgingBtn",this.AgingTextBlock},
                { "ReflowBtn",this.ReflowTextBlock},
                { "WaveBtn",this.WaveTextBlock},
                { "HighVoltageBtn",this.HighVoltageTextBlock},
                { "IntrinsicErrorBtn",this.IntrinsicErrorTextBlock},
                { "IntrinsicErrorDetailBtn",this.IntrinsicErrorDetailTextBlock},
                { "DayTimingBtn",this.DayTimingTextBlock},
            };
            var get_min_dateTask = eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(EIP_PRO_GlobalParameter.Eip_Min_Data_Create_and_Get);
            get_min_dateTask.Wait();
            get_min_date = get_min_dateTask.Result.Parameter;
        }


        public async Task SimulateTheScore(string btnName)
        {

            var dateBox = buttonDateGetMapper[btnName];
            if (!buttonProcedureMapper.ContainsKey(btnName))
            {
                Messenger.Default.Send("未找到该功能。", "SendMessageToMainWin");
                return;
            }
            var procedureName = buttonProcedureMapper[btnName];
            var workorder = this.WorkOrderTextbox;
            var date = dateBox();
            if (String.IsNullOrEmpty(workorder))
            {
                Messenger.Default.Send("请输入工单号。", "SendMessageToMainWin");
                return;
            }
            if (!date.HasValue)
            {
                Messenger.Default.Send("请选择日期。", "SendMessageToMainWin");
                return;
            }
            string sqlText = $"call {procedureName}(:pi_mo,:pi_date)";
            var param = new procedureParameter() { pi_mo = workorder, pi_date = date.Value.ToString("yyyyMMdd") };
            ProcedureGenericService procedureGenericService = new ProcedureGenericService(LocalConstant.OracleCurrentConnectionStringBuilder);
            try
            {
                var result = await procedureGenericService.CallProcedure<procedureParameter>(sqlText, param).ConfigureAwait(true);

                if (buttonTextBlockMapper.ContainsKey(btnName))
                {
                    var textBlock = buttonTextBlockMapper[btnName];
                    textBlock = $"影响的行数为:{result}";
                }
            }
            catch (Exception e1)
            {
                LocalConstant.Logger.Debug("AppDomainUnhandledExceptionHandler", e1);
                if (buttonTextBlockMapper.ContainsKey(btnName))
                {
                    var textBlock = buttonTextBlockMapper[btnName];
                    textBlock = $"{e1.Message},存储过程名字:{procedureName},数据库:{LocalConstant.oracleConnectionStringBuilderTest.DataSource}";
                }

            }
        }
        private void InitializeDate()
        {
            foreach (var zk in zkindMapper)
            {
                zkindActionMapper[zk.Value](zk.Key);
            }
        }
        public async Task VerifyMOAndGetSimulateDate()
        {
            mo = await mOSearchService.GetEntry(new MO() { MOName = this.WorkOrderTextbox }).ConfigureAwait(true);
            if (mo == null)
            {
                Messenger.Default.Send("该工单不存在于MES数据库。", "SendMessageToMainWin");
                return;
            }
            var result = await zGW_MO_TYPECreateService.Create(new ZGW_MO_TYPE() { TYPE = mo.ProductType, WORK_ORDER_CODE = mo.MOName }).ConfigureAwait(true);
            if (result == null)
            {
                Messenger.Default.Send("未能成功创建行到表ZGW_MO_TYPE。", "SendMessageToMainWin");
                return;
            }
            OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();
            dynamicParameters.Add("pi_mo", this.WorkOrderTextbox);
            dynamicParameters.Add("mycursor", dbType: OracleDbType.RefCursor, direction: ParameterDirection.Output);
            //var localQueryString = String.Format(get_min_date, this.WorkOrderTextbox.Text);

            dateList = await genericExecutionService.SGCCExecuteAndQueryAsync(get_min_date, dynamicParameters).ConfigureAwait(true);
            if (dateList.Count <= 0)
            {
                Messenger.Default.Send("未找到任何日期。", "SendMessageToMainWin");
            }
            InitializeDate();
        }


        public void ClearAllThePicker()
        {
            this.PCBAOIDatePicker = null;
            this.FCTDatePicker = null;
            this.BatteryCurrrentDatePicker = null;
            this.AgingDatePicker = null;
            this.ReflowDatePicker = null;
            this.WaveDatePicker = null;
            this.HighVoltageDatePicker = null;
            this.IntrinsicErrorDatePicker = null;
            this.IntrinsicErrorDetailDatePicker = null;
            this.DayTimingDatePicker = null;
        }

    }
    public class procedureParameter
    {
        public string pi_mo { get; set; }
        public string pi_date { get; set; }
    }
}
