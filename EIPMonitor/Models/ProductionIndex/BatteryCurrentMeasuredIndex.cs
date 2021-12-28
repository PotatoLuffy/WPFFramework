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
    public class BatteryCurrentMeasuredIndex: IPIScoreDetail
    {
        public BatteryCurrentMeasuredIndex(string workOrder, IEnumerable<PP_TEST_BATCUR> totalList, IEnumerable<PP_TEST_BATCUR> passList)
        {
            this.WorkOrder = workOrder;
            this.TotalList = totalList;
            this.PassList = passList;
        }
        [DisplayName("生产工单")]
        public String WorkOrder { get; set; }
        [Browsable(false)]
        public ProductionTestItem Item { get => ProductionTestItem.ProductionProcessInspection_BatteryCurrent; }
        [DisplayName("信息类别")]
        public String ItemName { get => Item.Translator(); }
        [Browsable(false)]
        public ProductionDataCategory Category { get => ProductionDataCategory.Producing; }
        [DisplayName("采集类别")]
        public String CategoryName { get => Category.Translator(); }
        [Browsable(false)]
        public ProductionDataItem ScoreItem { get => ProductionDataItem.CurrentMeasuredValue; }
        [DisplayName("数据项")]
        public String ScoreItemName { get => ScoreItem.Translator(); }
        [DisplayName("总行数")]
        public Int32 TotalCount { get => TotalList.Count(); }
        [DisplayName("通过的行数")]
        public Int32 PassCount { get => PassList.Count(); }
        [DisplayName("测量值之合")]
        public Decimal? SumTestValue { get => TotalList.Sum(s => s.CURRENT_VALUE);  }
        [DisplayName("标准分值")]
        public Int32 StandardScore { get => ProductionIndexStandardScore.GetStandardScore(Item, ScoreItem); }
        public Decimal Score { get => ProductionIndexPolicy.policyMapper[Category][Item][ScoreItem](Calculate.Devide(SumTestValue ?? 0.0m, TotalCount)); }
        [DisplayName("得分")]
        public IEnumerable<PP_TEST_BATCUR> TotalList { get; set; }
        public IEnumerable<PP_TEST_BATCUR> PassList { get; set; }
    }
}
