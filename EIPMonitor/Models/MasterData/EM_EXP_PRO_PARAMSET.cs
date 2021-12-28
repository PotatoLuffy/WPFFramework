using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Standard.Tool;
namespace EIPMonitor.Model.MasterData
{
    public class EM_EXP_PRO_PARAMSET
    {
        [NotMapped]
        [DisplayName("数据插入结果")]
        public String InsertedResult { get; set; }
        [NotMapped]
        [DisplayName("数据失败原因")]
        public String FailureReason { get; set; }
        //"STANDARDVERSION" NUMBER(11) NOT NULL,
        [DisplayName("国网版本")]
        public Decimal STANDARDVERSION { get; set; }
        //"PRODUCTION_ORDER_ID" VARCHAR2(32 BYTE) NOT NULL,
        [DisplayName("生产订单号")]
        public string PRODUCTION_ORDER_ID { get; set; }
        //"WORK_ORDER_CODE" VARCHAR2(32 BYTE) NOT NULL,
        [DisplayName("工单号")]
        public string WORK_ORDER_CODE { get => this.PRODUCTION_ORDER_ID; }
        //"TEST_CODE" VARCHAR2(32 BYTE) NOT NULL,
        [DisplayName("测试代码")]
        public string TEST_CODE { get; set; }
        //"PLANT_CODE" VARCHAR2(32 BYTE) ,
        [DisplayName("平台代码")]
        public string PLANT_CODE { get => this.PCB_CODE; }
        //"CHECK_TIME" DATE NOT NULL ,
        [DisplayName("检验时间")]
        public DateTime CHECK_TIME { get; set; }
        //"CREATE_TIME" DATE NOT NULL ,
        [DisplayName("创建时间")]
        public DateTime CREATE_TIME { get => this.CHECK_TIME; }
        //"MODEL_CODE" VARCHAR2(50 BYTE) NOT NULL,
        [DisplayName("规格型号")]
        public string MODEL_CODE { get; set; }
        //"INFO_TYPE_CODE" VARCHAR2(10 BYTE) NOT NULL,
        public string INFO_TYPE_CODE { get; set; } //default value is 1008?
        //"SOFTWARE_VERSION" VARCHAR2(50 BYTE) ,
        [DisplayName("软件版本号")]
        public string SOFTWARE_VERSION { get; set; }
        //"NAMEPLATE_CODE" VARCHAR2(50 BYTE) NOT NULL,
        [DisplayName("铭牌号")]
        public string NAMEPLATE_CODE { get => this.ASSET_MANAGEMENT_NO; }
        //"PARAMETER_NAME" VARCHAR2(50 BYTE) ,
        //"PARAMETER_VALUE" VARCHAR2(50 BYTE) ,
        //"READ_VALUE" VARCHAR2(50 BYTE) ,
        //"CONCLUSION" NUMBER(11) NOT NULL,
        [DisplayName("结论")]
        public Decimal CONCLUSION { get => 0; }
        //"FLAG" NUMBER(1) DEFAULT 0  ,
        //"PUTTIME" DATE ,
        //"TEST_NAME" VARCHAR2(50 BYTE) ,
        [DisplayName("测试名字")]
        public string TEST_NAME { get; set; }
        //"PCB_CODE" VARCHAR2(32 BYTE) ,
        [DisplayName("PCB编号")]
        public string PCB_CODE { get; set; }
        //"DEVICE_NO" VARCHAR2(32 BYTE) ,
        [DisplayName("设备编号")]
        public string DEVICE_NO { get; set; }
        //"UPLOAD_TIME" DATE ,

        //"TABLE_NO" VARCHAR2(50 BYTE) ,
        [DisplayName("表编号")]
        public string TABLE_NO { get => this.ASSET_MANAGEMENT_NO.Substring(Math.Abs(this.ASSET_MANAGEMENT_NO.Length - 12 - 1), 12); }
        //"TABLE_ADDRESS" VARCHAR2(50 BYTE) ,
        [DisplayName("表地址")]
        public string TABLE_ADDRESS { get => this.ASSET_MANAGEMENT_NO.Substring(Math.Abs(this.ASSET_MANAGEMENT_NO.Length - 12 - 1), 12); }
        [DisplayName("资产编号")]
        //"ASSET_MANAGEMENT_NO" VARCHAR2(50 BYTE)
        public string ASSET_MANAGEMENT_NO { get; set; }


        public static List<EM_EXP_PRO_PARAMSET> ReadDataFromDataTable(DataTable dataTable,string testName,string info_type_code,List<String> testCode,string standardVersion)
        {
            List<EM_EXP_PRO_PARAMSET> result = new List<EM_EXP_PRO_PARAMSET>();
            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                result.Add(new EM_EXP_PRO_PARAMSET()
                {
                    PRODUCTION_ORDER_ID = dataTable.Rows[row][0].ToString(),
                    DEVICE_NO = dataTable.Rows[row][1].ToString(),
                    MODEL_CODE = dataTable.Rows[row][2].ToString(),
                    PCB_CODE = dataTable.Rows[row][3].ToString(),
                    CHECK_TIME = dataTable.Rows[row][4].ToString().ConvertToDateTime(),
                    SOFTWARE_VERSION = dataTable.Rows[row][5].ToString(),
                    ASSET_MANAGEMENT_NO = dataTable.Rows[row][6].ToString(),
                    TEST_NAME = testName,
                    INFO_TYPE_CODE = info_type_code,
                    TEST_CODE = testCode.ContainIndex<string>(row)?[row] ?? String.Empty,
                    STANDARDVERSION = standardVersion.ConvertToDecimal()

                }) ;
                
            }
            return result;
        }
        
        
        
        
        

    }
}
