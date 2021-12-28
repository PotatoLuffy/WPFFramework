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
    public class HighVoltageInsulationTestIndex : IPIScoreDetail
    {
        public HighVoltageInsulationTestIndex(string workOrder, IEnumerable<EM_EXP_PRO_VOLTEST> totalList, IEnumerable<EM_EXP_PRO_VOLTEST> passList)
        {
            this.WorkOrder = workOrder;
            this.TotalList = totalList;
            this.PassList = passList;
        }
        public String WorkOrder { get; set; }
        public ProductionTestItem Item { get => ProductionTestItem.Experimentation_HighVoltageInsulationTestion; }
        public String ItemName { get => Item.Translator(); }
        public ProductionDataCategory Category { get => ProductionDataCategory.QualityTesting; }
        public String CategoryName { get => Category.Translator(); }
        public ProductionDataItem ScoreItem { get => ProductionDataItem.ElectricLeakageMeasuredValue; }
        public String ScoreItemName { get => ScoreItem.Translator(); }
        [DisplayName("总行数")]
        public Int32 TotalCount { get => TotalList.Count(); }
        [DisplayName("通过的行数")]
        public Int32 PassCount { get => PassList.Count(); }
        public Int32 StandardScore { get => ProductionIndexStandardScore.GetStandardScore(Item, ScoreItem); }
        public Decimal? SumTestValue { get => TotalList.Sum(s => s.LEAKAGE_CURRENT); }
        public Decimal Score { get => ProductionIndexPolicy.policyMapper[Category][Item][ScoreItem](Calculate.Devide(SumTestValue??0, TotalCount)); }
        public IEnumerable<EM_EXP_PRO_VOLTEST> TotalList { get; set; }
        public IEnumerable<EM_EXP_PRO_VOLTEST> PassList { get; set; }
    }
}
