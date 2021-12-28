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
    public class CrystalResonatorIndex : IPIScoreDetail
    {
        public CrystalResonatorIndex(string workOrder, IEnumerable<PD_MQC_CR> totalList, IEnumerable<PD_MQC_CR> passList)
        {
            this.WorkOrder = workOrder;
            this.TotalList = totalList;
            this.PassList = passList;
        }
        public String WorkOrder { get; set; }
        public ProductionTestItem Item { get => ProductionTestItem.CrystalResonator; }
        public String ItemName { get => Item.Translator(); }
        public ProductionDataCategory Category { get => ProductionDataCategory.Material; }
        public String CategoryName { get => Category.Translator(); }
        public ProductionDataItem ScoreItem { get => ProductionDataItem.StandardFrequentError; }
        public String ScoreItemName { get => ScoreItem.Translator(); }
        [DisplayName("总行数")]
        public Int32 TotalCount { get => TotalList.Count(); }
        [DisplayName("通过的行数")]
        public Int32 PassCount { get => PassList.Count(); }
        public Decimal? SumTestValue { get=>TotalList.Sum(s=>s.FREQUENCY_VALUE); }
        public Int32 StandardScore { get => ProductionIndexStandardScore.GetStandardScore(Item, ScoreItem); }
        public Decimal Score { get => ProductionIndexPolicy.policyMapper[Category][Item][ScoreItem](Calculate.Devide(SumTestValue??0, TotalCount)); }
        public IEnumerable<PD_MQC_CR> TotalList { get; set; }
        public IEnumerable<PD_MQC_CR> PassList { get; set; }
    }
}
