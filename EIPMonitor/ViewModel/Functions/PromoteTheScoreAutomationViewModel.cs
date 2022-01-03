using Dapper;
using EIPMonitor.Databse;
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.ViewModel.Functions
{
    public class PromoteTheScoreAutomationViewModel:ViewModelBase
    {
        private List<ZCL_SIMUL_D> details;
        private ObservableCollection<ZCL_SIMUL_DVM> detailsVM;
        private ZCL_SIMUL_DService zCL_SIMUL_DService = new ZCL_SIMUL_DService();
        private ZCL_TYPESearchService zCL_TYPESearchService = new ZCL_TYPESearchService();
        private List<ZCL_TYPE> standardZCL_TYPE;
        private MES_MO_TO_EIP_POOL mES_MO_TO_EIP_POOL;
        private ZGW_MO_T moInfo;
        private readonly string woVerifySqlText = "select * from ZGW_MO_T where IPONO = :IPONO and rownum = 1";
        private readonly GenericExecutionService<ZGW_MO_T> genericExecutionService = new GenericExecutionService<ZGW_MO_T>();

        private string materialCodeTextBlock;
        private string materialNameTextBlock;
        private string orderQtyTextBlock;
        private string experimentScoreTextBlock;
        private string materialScoreTextBlock;
        private string produceProcessScoreTextBlock;
        private string totalScoreTextBlock;
        private string qualityScoreTextBlock;
        private string orderTextBox;
        public String OrderTextBox { get => orderTextBox; set => SetProperty(ref orderTextBox, value); }
        public String MaterialCodeTextBlock { get => materialCodeTextBlock; set => SetProperty(ref materialCodeTextBlock, value); }
        public String MaterialNameTextBlock { get => materialNameTextBlock; set => SetProperty(ref materialNameTextBlock, value); }
        public String OrderQtyTextBlock { get => orderQtyTextBlock; set => SetProperty(ref orderQtyTextBlock, value); }
        public String ExperimentScoreTextBlock { get => experimentScoreTextBlock; set => SetProperty(ref experimentScoreTextBlock, value); }
        public String MaterialScoreTextBlock { get => materialScoreTextBlock; set => SetProperty(ref materialScoreTextBlock, value); }
        public String ProduceProcessScoreTextBlock { get => produceProcessScoreTextBlock; set => SetProperty(ref produceProcessScoreTextBlock, value); }
        public String TotalScoreTextBlock { get => totalScoreTextBlock; set => SetProperty(ref totalScoreTextBlock, value); }
        public String QualityScoreTextBlock { get => qualityScoreTextBlock; set => SetProperty(ref qualityScoreTextBlock, value); }
        public ObservableCollection<ZCL_SIMUL_DVM> DetailsVM { get => detailsVM; set => SetProperty(ref detailsVM, value); }
        private readonly char startLetter = LocalConstant.IsAdmin ? '%' : '6';
        public PromoteTheScoreAutomationViewModel()
        {
        }
        public async Task GetMoInfo()
        {
            moInfo = await VerifyTheMOName(this.OrderTextBox).ConfigureAwait(true);
            if (moInfo == null) return;
            await GrabInformationOfWorkOrder(moInfo.IPONO).ConfigureAwait(true);
            RegisterEvent();
        }
        private async Task GrabInformationOfWorkOrder(string workOrder)
        {
            details = await zCL_SIMUL_DService.GetEntries(workOrder, workOrder, null, startLetter).ConfigureAwait(true);
            if (details == null || details.Count <= 0)
            {
                Messenger.Default.Send("未找到任何数据", "SendMessageToMainWin");
                return;
            }

            await CompensantTheDetails(details).ConfigureAwait(true);
            mES_MO_TO_EIP_POOL = details.Aggregate(IocKernel.Get<IUserStamp>()).DefaultIfEmpty().FirstOrDefault();
            initializeTheTitlePanel(mES_MO_TO_EIP_POOL, moInfo);
            this.DetailsVM = details.DeepClone<List<ZCL_SIMUL_D>, ObservableCollection<ZCL_SIMUL_DVM>>();
        }
        private void initializeTheTitlePanel(MES_MO_TO_EIP_POOL mES_MO_TO_EIP_POOL, ZGW_MO_T zGW_MO_T)
        {
            if (zGW_MO_T != null)
            {
                this.MaterialCodeTextBlock = zGW_MO_T.MATERIALSCODE;
                this.MaterialNameTextBlock = zGW_MO_T.MATERIALSNAME;
                this.OrderQtyTextBlock = zGW_MO_T.AMOUNT;
            }

            this.ExperimentScoreTextBlock = mES_MO_TO_EIP_POOL.EXPERIMENT_SCORE.ToString();
            this.MaterialScoreTextBlock = mES_MO_TO_EIP_POOL.MATERIAL_SCORE.ToString();
            this.ProduceProcessScoreTextBlock = mES_MO_TO_EIP_POOL.PRODUCE_PROCESS_SCORE.ToString();
            this.TotalScoreTextBlock = mES_MO_TO_EIP_POOL.SCORE.ToString();
            this.QualityScoreTextBlock = mES_MO_TO_EIP_POOL.QUALITY_EVALUATION;

        }
        public async Task<ZGW_MO_T> VerifyTheMOName(string moName)
        {
            if (!moName.StartsWith("5"))
            {
                Messenger.Default.Send("请输入以6开头的自动化工单。", "SendMessageToMainWin");
                return null;
            }
            ZGW_MO_T workOrder = new ZGW_MO_T() { IPONO = moName };
            var verifyResult = await genericExecutionService.SGCCQueryAsync(woVerifySqlText, workOrder).ConfigureAwait(true);


            if (verifyResult == null)
            {
                Messenger.Default.Send("未找到该工单相关信息。", "SendMessageToMainWin");
                return null;
            }
            return verifyResult;
        }
        private async Task CompensantTheDetails(List<ZCL_SIMUL_D> zCL_SIMUL_Ds)
        {
            if (standardZCL_TYPE == null)
                standardZCL_TYPE = await zCL_TYPESearchService.GetZCL_Type(new ZCL_TYPE() { ZVALID = 1, SUBCLASSCODE = details.FirstOrDefault().SUBCLASSCODE }).ConfigureAwait(true);
            var existed = zCL_SIMUL_Ds.DeepClone<List<ZCL_SIMUL_D>, List<ZCL_TYPE>>();
            var donTExisted = standardZCL_TYPE.Except(existed);
            foreach (var donTEl in donTExisted)
            {
                zCL_SIMUL_Ds.Add(donTEl.GetDefaultZCL_SIMUL_D(details.FirstOrDefault().WORK_ORDER_CODE));
            }
        }
        public async Task PromoteTheScoreForcely()
        {
            var selectedEntries = this.DetailsVM.Where(w => w.CheckBox == true).ToList();
            foreach (var selectedEntry in selectedEntries)
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("zcode", selectedEntry.ZKIND);
                dynamicParameters.Add("mo", selectedEntry.WORK_ORDER_CODE);
                dynamicParameters.Add("flag", "1");
                await genericExecutionService.SGCCExecuteAsync("call EIP_Score_Increase(:zcode,:mo,:flag)", dynamicParameters).ConfigureAwait(true);
            }
            await GrabInformationOfWorkOrder(moInfo.IPONO).ConfigureAwait(true);
        }
        public bool? IsAllItems1Selected
        {
            get
            {
                var selected = this.DetailsVM?.Select(item => item.CheckBox).Distinct().ToList() ?? default;

                return (selected?.Count ?? 0) == 1 ? selected.Single() : (bool?)null;
            }
            set
            {
                if (value.HasValue)
                {
                    SelectAll(value.Value, this.DetailsVM);
                    OnPropertyChanged();
                }
            }
        }
        private static void SelectAll(bool select, IEnumerable<ZCL_SIMUL_DVM> models)
        {
            if (models == null || models.Count() <= 0) return;
            foreach (var model in models)
            {
                model.CheckBox = select;
            }
        }
        private void RegisterEvent()
        {
            foreach (var model in this.DetailsVM)
            {
                model.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(ZCL_SIMUL_DVM.CheckBox))
                        OnPropertyChanged(nameof(IsAllItems1Selected));
                };
            }
        }
    }
}
