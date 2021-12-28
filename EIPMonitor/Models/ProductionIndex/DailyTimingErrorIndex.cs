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
    public class DailyTimingErrorIndex : IPIScoreDetail
    {
        public DailyTimingErrorIndex(string workOrder, IEnumerable<EM_EXP_PRO_DAYTIMEER> totalList, IEnumerable<EM_EXP_PRO_DAYTIMEER> passList)
        {
            this.WorkOrder = workOrder;
            this.TotalList = totalList;
            this.PassList = passList;
        }
        public String WorkOrder { get; set; }
        public ProductionTestItem Item { get => ProductionTestItem.Experimentation_DailyTimingEror; }
        public String ItemName { get => Item.Translator(); }
        public ProductionDataCategory Category { get => ProductionDataCategory.QualityTesting; }
        public String CategoryName { get => Category.Translator(); }
        public ProductionDataItem ScoreItem { get => ProductionDataItem.InspectionConclusion; }
        public String ScoreItemName { get => ScoreItem.Translator(); }
        [DisplayName("总行数")]
        public Int32 TotalCount { get => TotalList.Count(); }
        [DisplayName("通过的行数")]
        public Int32 PassCount { get => PassList.Count(); }
        public Int32 StandardScore { get => ProductionIndexStandardScore.GetStandardScore(Item, ScoreItem); }
        public Decimal? SumTestValue { get; }
        public Decimal Score { get => ProductionIndexPolicy.policyMapper[Category][Item][ScoreItem](Calculate.Devide(PassCount, TotalCount)); }
        public IEnumerable<EM_EXP_PRO_DAYTIMEER> TotalList { get; set; }
        public IEnumerable<EM_EXP_PRO_DAYTIMEER> PassList { get; set; }
    }
}
