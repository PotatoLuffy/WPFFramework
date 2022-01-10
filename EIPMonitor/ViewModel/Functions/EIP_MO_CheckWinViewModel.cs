using EIPMonitor.Domain.CustomException;
using EIPMonitor.DomainServices.MasterData;
using EIPMonitor.DomainServices.MasterData.MES_MO_TO_EIP_POOLServices;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.MasterData;
using EIPMonitor.Model.Widget;
using GalaSoft.MvvmLight.Messaging;
using Infrastructure.Standard.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EIPMonitor.ViewModel.Functions
{
    public class EIP_MO_CheckWinViewModel:ViewModelBase
    {
        private ObservableCollection<MES_MO_TO_EIP_POOLVM> mES_MO_TO_EIP_POOLs;
        private ZCL_SIMUL_DService zCL_SIMUL_DService = new ZCL_SIMUL_DService();
        private MES_MO_TO_EIP_POOLCheckService mES_MO_TO_EIP_POOLCheckService = new MES_MO_TO_EIP_POOLCheckService();
        private Boolean IsClosed = false;
        private ZCL_TYPESearchService zCL_TYPESearchService = new ZCL_TYPESearchService();
        private MES_MO_TO_EIP_POOLSearchService mES_MO_TO_EIP_POOLSearchService = new MES_MO_TO_EIP_POOLSearchService();
        private List<ZCL_SIMUL_D> selectedDetails;

        public ObservableCollection<MES_MO_TO_EIP_POOLVM> MES_MO_TO_EIP_POOLs { get => mES_MO_TO_EIP_POOLs; set => SetProperty(ref mES_MO_TO_EIP_POOLs, value); }
        public List<ZCL_SIMUL_D> SelectedDetails { get => selectedDetails; set => SetProperty(ref selectedDetails, value); }
        private string workOrderFromTextBox;
        private string workOrderToTextBox;
        public string WorkOrderFromTextBox { get => workOrderFromTextBox; set => SetProperty(ref workOrderFromTextBox, value); }
        public string WorkOrderToTextBox { get => workOrderToTextBox; set => SetProperty(ref workOrderToTextBox, value); }
        public List<string> CopiedOrders { get; set; }
        private readonly char startLetter = (LocalConstant.IsAdmin ? '%' : '5');
        public EIP_MO_CheckWinViewModel()
        {

        }
        public async Task MOListQuery()
        {
            try
            {
                if ((CopiedOrders?.Count ?? 0) >= 1)
                {
                    await GetScoresAndDetail(null, null, this.CopiedOrders).ConfigureAwait(false);
                    return;
                }
                if (String.IsNullOrWhiteSpace(WorkOrderFromTextBox) && String.IsNullOrWhiteSpace(WorkOrderToTextBox) && !LocalConstant.IsAdmin)
                {
                    MessageBox.Show("未找到任何工单。");
                    return;
                }
                var ifBeginOrder = Int32.TryParse(workOrderFromTextBox, out Int32 beginO);
                var ifEndOrder = Int32.TryParse(workOrderToTextBox, out Int32 enDO);

                await GetScoresAndDetail(ifBeginOrder ? workOrderFromTextBox : workOrderToTextBox, ifEndOrder ? workOrderToTextBox : workOrderFromTextBox, null).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                Messenger.Default.Send(e.Message, "SendMessageToMainWin");
            }
        }
        private async Task GetScoresAndDetail(string beginOrder, string endOrder, List<string> orders)
        {
            if (orders != null && orders.Count >= 1)

                this.MES_MO_TO_EIP_POOLs = (await mES_MO_TO_EIP_POOLSearchService.GeMES_MO_TO_EIP_POOL(orders, startLetter).ConfigureAwait(true)).DeepClone<List<MES_MO_TO_EIP_POOL>, ObservableCollection<MES_MO_TO_EIP_POOLVM>>();
            else
                this.MES_MO_TO_EIP_POOLs = (await mES_MO_TO_EIP_POOLSearchService.GetQueryData(beginOrder, endOrder, startLetter).ConfigureAwait(true)).DeepClone<List<MES_MO_TO_EIP_POOL>, ObservableCollection<MES_MO_TO_EIP_POOLVM>>();
            RegisterEvent();
        }
        public async Task CheckSelectedRows()
        {
            try
            {
                var localList = this.MES_MO_TO_EIP_POOLs.DeepClone().Where(w => w.ExistsFlag == false && w.CheckBox == true).ToList().DeepClone<List<MES_MO_TO_EIP_POOLVM>, List<MES_MO_TO_EIP_POOL>>();
                var result = await mES_MO_TO_EIP_POOLCheckService.Check(localList.Where(w => w.SCORE >= 85m).ToList(), IocKernel.Get<IUserStamp>()).ConfigureAwait(true);
                this.MES_MO_TO_EIP_POOLs = result.DeepClone<List<MES_MO_TO_EIP_POOL>, ObservableCollection<MES_MO_TO_EIP_POOLVM>>();
                RegisterEvent();
            }
            catch (OAResponseFailureException e)
            {
                Messenger.Default.Send(e.Message, "SendMessageToMainWin");
            }
            catch (Exception e1)
            {
                Messenger.Default.Send(e1.Message, "SendMessageToMainWin");
            }
        }
        public bool? IsAllItems1Selected
        {
            get
            {
                var selected = this.MES_MO_TO_EIP_POOLs?.Select(item => item.CheckBox).Distinct().ToList()??default;

                return (selected?.Count??0) == 1 ? selected.Single() : (bool?)null;
            }
            set
            {
                if (value.HasValue)
                {
                    SelectAll(value.Value, this.MES_MO_TO_EIP_POOLs);
                    OnPropertyChanged();
                }
            }
        }
        private static void SelectAll(bool select, IEnumerable<MES_MO_TO_EIP_POOLVM> models)
        {
            if (models == null || models.Count() <= 0) return;
            foreach (var model in models)
            {
                model.CheckBox = select;
            }
        }
        private void RegisterEvent()
        {
            foreach (var model in this.MES_MO_TO_EIP_POOLs)
            {
                model.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(MES_MO_TO_EIP_POOLVM.CheckBox))
                        OnPropertyChanged(nameof(IsAllItems1Selected));
                };
            }
        }
    }
}
