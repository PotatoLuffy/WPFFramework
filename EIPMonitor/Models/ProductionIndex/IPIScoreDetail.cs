using EIPMonitor.Model.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.ProductionIndex
{
    public interface IPIScoreDetail
    {
        [DisplayName("生产工单")]
        String WorkOrder { get; set; }
        [Browsable(false)]
         ProductionTestItem Item { get;  }
        [DisplayName("采集类别")]
        String CategoryName { get; }
        [DisplayName("信息类别")]
         String ItemName { get; }
        [Browsable(false)]
         ProductionDataCategory Category { get;  }
        [Browsable(false)]
         ProductionDataItem ScoreItem { get;  }
        [DisplayName("数据项")]
         String ScoreItemName { get; }
        [DisplayName("总行数")]
         Int32 TotalCount { get; }
        [DisplayName("通过的行数")]
         Int32 PassCount { get; }
        [DisplayName("测量值之合")]
         Decimal? SumTestValue { get; }
        [DisplayName("标准分值")]
         Int32 StandardScore { get; }
        [DisplayName("得分")]
         Decimal Score { get; }
    }
}
