using EIPMonitor.Model.SGCC;
using EIPMonitor.Model.Widget;
using Infrastructure.Standard.Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.ProductionIndex
{
    public class VoltageDependentResistorIndex : IPIScoreDetail
    {
        public VoltageDependentResistorIndex(string workOrder, IEnumerable<PD_MQC_VDR> totalList, IEnumerable<PD_MQC_VDR> passList)
        {
            this.WorkOrder = workOrder;
            this.TotalList = totalList;
            this.PassList = passList;
        }
        public String WorkOrder { get; set; }
        public ProductionTestItem Item { get => ProductionTestItem.VoltageDependentResistor; }
        public String ItemName { get => Item.Translator(); }
        public ProductionDataCategory Category { get => ProductionDataCategory.Material; }
        public String CategoryName { get => Category.Translator(); }
        public ProductionDataItem ScoreItem { get => ProductionDataItem.ErrorValue; }
        public String ScoreItemName { get => ScoreItem.Translator(); }
        [DisplayName("总行数")]
        public Int32 TotalCount { get => TotalList.Count(); }
        [DisplayName("通过的行数")]
        public Int32 PassCount { get => PassList.Count(); }
        public Decimal? SumTestValue { get =>TotalList.Sum(s=>s.ERROR_VALUE); }
        public Decimal Score { get => ProductionIndexPolicy.policyMapper[Category][Item][ScoreItem]( Math.Abs(Calculate.Devide(SumTestValue??0, TotalCount))); }
        public IEnumerable<PD_MQC_VDR> TotalList { get; set; }
        public IEnumerable<PD_MQC_VDR> PassList { get; set; }
        public Int32 StandardScore { get => ProductionIndexStandardScore.GetStandardScore(Item, ScoreItem); }
    }
}
