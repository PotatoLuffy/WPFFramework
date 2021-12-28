using EIPMonitor.Model.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.ProductionIndex
{
    public class ProductionIndexMain
    {
        [DisplayName("生产工单")]
        public String WorkOrder { get; set; }
        [DisplayName("原材料检验评分")]
        public Decimal MaterialInspectionScore { get; set; }
        [DisplayName("生产过程评分")]
        public Decimal ProductionProcessScore { get; set; }
        [DisplayName("实验过程评分")]
        public Decimal ExperimentScore { get; set; }

        [DisplayName("综合得分")]
        public Decimal Score { get; set; }
        [DisplayName("质量等级")]
        public String QualityLevel { get{
                if (this.Score > 80 && this.Score <= 100) return "优质品";
                if (this.Score > 40 && this.Score <= 80) return "良好品";
                return "合格品";
            } }
        

    }
}
