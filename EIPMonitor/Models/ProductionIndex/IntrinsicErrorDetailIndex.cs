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
    public class IntrinsicErrorDetailIndex : IPIScoreDetail
    {
        public IntrinsicErrorDetailIndex(string workOrder, IEnumerable<EM_EXP_PRO_BASICER_DETAIL> totalList, IEnumerable<EM_EXP_PRO_BASICER_DETAIL> passList)
        {
            this.WorkOrder = workOrder;
            this.TotalList = totalList;
            this.PassList = passList;
        }
        public String WorkOrder { get; set; }

        public ProductionTestItem Item { get => ProductionTestItem.Experimentation_IntrinsicErrorDetail; }
        public String ItemName { get => Item.Translator(); }
        public ProductionDataCategory Category { get => ProductionDataCategory.QualityTesting; }
        public String CategoryName { get => Category.Translator(); }
        public ProductionDataItem ScoreItem { get => ProductionDataItem.AverageError; }
        public String ScoreItemName { get => ScoreItem.Translator(); }
        [DisplayName("总行数")]
        public Int32 TotalCount { get => TotalList.Count(); }
        [DisplayName("通过的行数")]
        public Int32 PassCount { get => PassList.Count(); }
        public Decimal? SumTestValue { get => TotalList.Sum(s => s.AVERAGE_ERROR.ConvertToDecimal()); }
        public Int32 StandardScore { get => ProductionIndexStandardScore.GetStandardScore(Item, ScoreItem); }
        public Decimal Score { get => ProductionIndexPolicy.policyMapper[Category][Item][ScoreItem](Math.Abs( Calculate.Devide(SumTestValue??0, TotalCount))); }
        public IEnumerable<EM_EXP_PRO_BASICER_DETAIL> TotalList { get; set; }
        public IEnumerable<EM_EXP_PRO_BASICER_DETAIL> PassList { get; set; }
    }
}
