using EIPMonitor.ViewModel;
using Infrastructure.Standard.Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.Widget
{
    public class ZCL_SIMUL_DVM:ViewModelBase
    {
        private bool _checkBox;
        [DisplayName("选择")]
        public Boolean CheckBox { get => _checkBox; set => SetProperty(ref _checkBox, value); }
        [DisplayName("大类")]
        public string ZTYPE { get; set; }
        //类别
        // "ZKIND" VARCHAR2(10 BYTE) NOT NULL,
        //public string ZKIND { get; set; }
        ////类别描述
        //// "ZDESC" VARCHAR2(50 BYTE) ,
        [DisplayName("类别")]
        public String ZDESC { get; set; }
        //"WORK_ORDER_CODE" VARCHAR2(32 BYTE) NOT NULL,
        //工单编号
        [DisplayName("工单号")]
        public String WORK_ORDER_CODE { get; set; }
        // "SUBCLASSCODE" VARCHAR2(10 BYTE) NOT NULL,
        //种类编码
        public String SUBCLASSCODE { get; set; }
        //类别
        // "ZKIND" VARCHAR2(10 BYTE) NOT NULL,
        [DisplayName("类别")]
        public String ZKIND { get; set; }
        // "ACCEPT" NUMBER(13,2) ,
        //合格数
        public Decimal ACCEPT { get; set; }
        [DisplayName("合格数")]
        public String OKOutput { get { if (this.ZFLAG == 0) return ACCEPT.ToString("###,###.####"); return "N/A"; } }
        [DisplayName("实测值")]
        public String MeasuredValue { get { if (this.ZFLAG == 1) return ACCEPT.ConvertToFormatterDouble(2); return "N/A"; } }
        // "TOTAL" NUMBER(13,2) ,
        //总数
        public Decimal TOTAL { get; set; }
        [DisplayName("总数")]
        public String TOTALNAME { get => this.TOTAL.ToString("###,###.####"); }
        //合格率
        // "RATE" NUMBER(13,2) ,
        public Decimal RATE { get; set; }
        [DisplayName("合格率")]
        public String RATENAME { get { if (this.ZFLAG == 1) return Calculate.Devide(this.RATE, this.ZFLAG * 100).ConvertToFormatterDouble(2); if (this.RATE == 0) return "0%"; return this.RATE.ConvertToFormatterDouble(2) + "%"; } }
        //标准分值
        // "ZDSCORE" NUMBER ,
        [DisplayName("标准分值")]
        public Decimal ZDSCORE { get; set; }
        //分数
        // "SCORE" NUMBER(13,2) ,
        [DisplayName("得分")]
        public Decimal SCORE { get; set; }
        // "STATUS" VARCHAR2(10 BYTE) DEFAULT 'X'
        //状态
        public String STATUS { get; set; }
        // "CREATETIME" DATE DEFAULT sysdate
        //计算日期
        public DateTime CREATETIME { get; set; }

        //public string SUBCLASSCODE { get; set; }
        ////类型
        //// "ZTYPE" VARCHAR2(30 BYTE) NOT NULL,

        //标志 1 需要 除以100
        // "ZFLAG" NUMBER DEFAULT 0
        public Decimal ZFLAG { get; set; }
        //失效 0 表示失效
        // "ZVALID" NUMBER DEFAULT 1
        public Decimal ZVALID { get; set; }

        //版本号
        // "ZVER" NUMBER NOT NULL
        public Decimal ZVER { get; set; }
        public Decimal ZWEIGHT { get; set; }
        [NotMapped]
        public string MATERIALSNAME { get; set; }
    }
}
