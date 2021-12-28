using Infrastructure.Standard.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.MasterData
{
    public class ZCL_TYPE : IEquatable<ZCL_TYPE>
    {
        // 种类编码
        //         "SUBCLASSCODE" VARCHAR2(10 BYTE) NOT NULL,
        public string SUBCLASSCODE { get; set; }
        //类型
        // "ZTYPE" VARCHAR2(30 BYTE) NOT NULL,
        public string ZTYPE { get; set; }
        //类别
        // "ZKIND" VARCHAR2(10 BYTE) NOT NULL,
        public string ZKIND { get; set; }
        //类别描述
        // "ZDESC" VARCHAR2(50 BYTE) ,
        public String ZDESC { get; set; }
        //标志 1 需要 除以100
        // "ZFLAG" NUMBER DEFAULT 0
        public Decimal ZFLAG { get; set; }
        //失效 0 表示失效
        // "ZVALID" NUMBER DEFAULT 1
        public Decimal ZVALID { get; set; }
        //标准分值
        // "ZDSCORE" NUMBER ,
        public Decimal ZDSCORE { get; set; }
        //版本号
        // "ZVER" NUMBER NOT NULL
        public Decimal ZVER { get; set; }
        public Decimal ZWEIGHT { get; set; }

        public bool Equals(ZCL_TYPE other)
        {
            if (other == null) return false;
            if (this.SUBCLASSCODE.Equals(other.SUBCLASSCODE) && this.ZTYPE.Equals(other.ZTYPE) && this.ZKIND.Equals(other.ZKIND)) return true;
            return false;
        }
        public override bool Equals(object obj)
        {
            ZCL_TYPE other = obj as ZCL_TYPE;
            return this.Equals(other);
        }
        public override int GetHashCode()
        {
            return $"{this.SUBCLASSCODE}-{this.ZTYPE}-{this.ZKIND}".GetHashCode();
        }
        public ZCL_SIMUL_D GetDefaultZCL_SIMUL_D(string workOrder)
        {
            var result = this.DeepClone<ZCL_TYPE, ZCL_SIMUL_D>();
            result.WORK_ORDER_CODE = workOrder;
            result.ACCEPT = 0;
            result.RATE = 0;
            result.SCORE = 0;
            return result;
        }
    }
}
