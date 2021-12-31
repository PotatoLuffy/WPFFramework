using EIPMonitor.DomainServices.MasterData;
using EIPMonitor.DomainServices.MasterData.MES_MO_TO_EIP_POOLServices;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.MasterData;
using GalaSoft.MvvmLight.Messaging;
using Infrastructure.Standard.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EIPMonitor.ViewModel.Functions
{
    public class ProductionIndexQueryAutomationViewModel : ViewModelBase
    {
        private List<ZCL_SIMUL_D> details;
        private List<ZCL_SIMUL_D> selectedDetails;
        private List<ZCL_TYPE> standardZCL_TYPE;
        private List<MES_MO_TO_EIP_POOL> mES_MO_TO_EIP_POOLs;
        private ZCL_SIMUL_DService zCL_SIMUL_DService = new ZCL_SIMUL_DService();
        private MES_MO_TO_EIP_POOLCreateService mES_MO_TO_EIP_POOLCreateService = new MES_MO_TO_EIP_POOLCreateService();
        private Boolean IsClosed = false;
        private ZCL_TYPESearchService zCL_TYPESearchService = new ZCL_TYPESearchService();
        private MES_MO_TO_EIP_POOLSearchService mES_MO_TO_EIP_POOLSearchService = new MES_MO_TO_EIP_POOLSearchService();

        public List<MES_MO_TO_EIP_POOL> MES_MO_TO_EIP_POOLs { get => mES_MO_TO_EIP_POOLs; set => SetProperty(ref mES_MO_TO_EIP_POOLs, value); }
        public List<ZCL_SIMUL_D> Details { get => details; set => details = value; }
        public List<ZCL_SIMUL_D> SelectedDetails { get => selectedDetails; set => SetProperty(ref selectedDetails, value); }
        private string workOrderFromTextBox;
        private string workOrderToTextBox;
        public string WorkOrderFromTextBox { get => workOrderFromTextBox; set => SetProperty(ref workOrderToTextBox, value); }
        public string WorkOrderToTextBox { get => workOrderToTextBox; set => SetProperty(ref workOrderToTextBox, value); }
        public List<string> CopiedOrders { get; set; }
        private readonly char startLetter = (LocalConstant.IsAdmin ? '%' : '5');
        public ProductionIndexQueryAutomationViewModel()
        {
        }
        public async Task MOListQuery()
        {
            try
            {
                if ((CopiedOrders?.Count ?? 0) >= 1)
                {
                    await GetScoresAndDetail(null, null, this.CopiedOrders).ConfigureAwait(false);
                }
                if (String.IsNullOrWhiteSpace(workOrderFromTextBox) && String.IsNullOrWhiteSpace(workOrderToTextBox))
                {
                    Messenger.Default.Send("未找到任何工单。", "SendMessageToMainWin");
                    return;
                }
                var ifBeginOrder = Int32.TryParse(workOrderFromTextBox, out Int32 beginO);
                var ifEndOrder = Int32.TryParse(workOrderToTextBox, out Int32 enDO);
                Details = await zCL_SIMUL_DService.GetEntries(ifBeginOrder ? workOrderFromTextBox : workOrderToTextBox, ifEndOrder ? workOrderToTextBox : workOrderFromTextBox, null, startLetter).ConfigureAwait(true);

                if (Details == null || Details.Count <= 0)
                {
                    Messenger.Default.Send("未找到任何工单。", "SendMessageToMainWin");
                    return;
                }

                await CreateScoresToDB(this.MES_MO_TO_EIP_POOLs).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Messenger.Default.Send(e.Message, "SendMessageToMainWin");
            }

        }
        private async Task GetScoresAndDetail(string beginOrder, string endOrder, List<string> orders)
        {
            Details = await zCL_SIMUL_DService.GetEntries(beginOrder, endOrder, orders, startLetter).ConfigureAwait(true);
            this.mES_MO_TO_EIP_POOLs = details.Aggregate(IocKernel.Get<IUserStamp>());
        }
        private async Task CreateScoresToDB(List<MES_MO_TO_EIP_POOL> mES_MO_TO_EIP_POOLsPara)
        {
            var uploadedEntries = await mES_MO_TO_EIP_POOLSearchService.GeMES_MO_TO_EIP_POOL(mES_MO_TO_EIP_POOLs).ConfigureAwait(true);
            foreach (var el in uploadedEntries)
            {
                var inx = mES_MO_TO_EIP_POOLs.FindIndex(f => f.Equals(el));
                mES_MO_TO_EIP_POOLs[inx].ExistsFlag = el.ExistsFlag;
            }
            this.MES_MO_TO_EIP_POOLs = mES_MO_TO_EIP_POOLs.OrderBy(o => o.SCORE).ToList();
            await CreateMES_MO_TO_EIP_POOL().ConfigureAwait(false);
        }
        public async Task GetSelectedEntryDetails(MES_MO_TO_EIP_POOL mES_MO_TO_EIP_POOL)
        {
            var localList = details.Where(w => w.WORK_ORDER_CODE == mES_MO_TO_EIP_POOL.PRODUCTION_ORDER_ID).OrderBy(o => o.WORK_ORDER_CODE).ThenBy(t => t.ZTYPE).ThenBy(t => t.ZDESC).ToList();

            if (standardZCL_TYPE == null)
                standardZCL_TYPE = await zCL_TYPESearchService.GetZCL_Type(new ZCL_TYPE() { ZVALID = 1, SUBCLASSCODE = details.FirstOrDefault().SUBCLASSCODE }).ConfigureAwait(true);
            var existed = localList.DeepClone<List<ZCL_SIMUL_D>, List<ZCL_TYPE>>();
            var donTExisted = standardZCL_TYPE.Except(existed);
            foreach (var donTEl in donTExisted)
            {
                localList.Add(donTEl.GetDefaultZCL_SIMUL_D(details.FirstOrDefault().WORK_ORDER_CODE));
            }
            this.SelectedDetails = localList;
        }
        private async Task CreateMES_MO_TO_EIP_POOL()
        {
            var localList = this.mES_MO_TO_EIP_POOLs.DeepClone().ToList();
            var result = await mES_MO_TO_EIP_POOLCreateService.Create(localList, IocKernel.Get<IUserStamp>()).ConfigureAwait(true);
        }

    }
}
