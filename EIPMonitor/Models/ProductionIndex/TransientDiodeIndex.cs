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
    public class TransientDiodeIndex : IPIScoreDetail
    {
        public TransientDiodeIndex(string workOrder, IEnumerable<PD_MQC_TVS> totalList, IEnumerable<PD_MQC_TVS> passList)
        {
            this.WorkOrder = workOrder;
            this.TotalList = totalList;
            this.PassList = passList;
        }
        public String WorkOrder { get; set; }
        public ProductionTestItem Item { get => ProductionTestItem.TransientDiode; }
        public String ItemName { get => Item.Translator(); }
        public ProductionDataCategory Category { get=> ProductionDataCategory.Material;  }
        public String CategoryName { get => Category.Translator(); }
        public ProductionDataItem ScoreItem { get=> ProductionDataItem.InspectionConclusion;  }
        public String ScoreItemName { get => ScoreItem.Translator(); }
        [DisplayName("总行数")]
        public Int32 TotalCount { get => TotalList.Count(); }
        [DisplayName("通过的行数")]
        public Int32 PassCount { get => PassList.Count(); }
        public Decimal? SumTestValue { get; }
        public Decimal Score { get => ProductionIndexPolicy.policyMapper[Category][Item][ScoreItem](Calculate.Devide(PassCount, TotalCount)); }
        public IEnumerable<PD_MQC_TVS> TotalList { get; set; }
        public IEnumerable<PD_MQC_TVS> PassList { get; set; }
        public Int32 StandardScore { get => ProductionIndexStandardScore.GetStandardScore(Item, ScoreItem); }
    }
}
