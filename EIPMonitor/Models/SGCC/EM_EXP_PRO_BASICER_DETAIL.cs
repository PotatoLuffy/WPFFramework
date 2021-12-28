using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.SGCC
{
    public class EM_EXP_PRO_BASICER_DETAIL
    {
        //        TEST_NAME VARCHAR2			50
        //PCB_CODE VARCHAR2			32
        //DEVICE_NO VARCHAR2			32
        //UPLOAD_TIME DATE			7
        public DateTime UPLOAD_TIME { get; set; }
        //AVERAGE_ERROR_TYPE VARCHAR2			5
        //STANDARDVERSION NUMBER			22
        public Int32 STANDARDVERSION { get; set; }
        //PRODUCTION_ORDER_ID VARCHAR2			32
        public String PRODUCTION_ORDER_ID { get; set; }
        //WORK_ORDER_CODE VARCHAR2			32
        //TEST_CODE VARCHAR2			32
        public String TEST_CODE { get; set; }
        //PLANT_CODE VARCHAR2			32
        //CHECK_TIME DATE			7
        //CREATE_TIME DATE			7
        //MODEL_CODE VARCHAR2			50
        //INFO_TYPE_CODE VARCHAR2			10
        //POWER VARCHAR2			5
        //DIRECTION VARCHAR2			5
        //YUAN VARCHAR2			5
        //TESTING_VOLTAGE VARCHAR2			10
        //TESTING_CURRENT VARCHAR2			10
        //POWER_FACTOR VARCHAR2			10
        //AVERAGE_ERROR VARCHAR2			10
        public string AVERAGE_ERROR { get; set; }
        //CONCLUSION NUMBER			22
        public Int32 CONCLUSION { get; set; }
        //FLAG NUMBER			22
        //PUTTIME DATE			7
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String BeginOrder { get; set; }
        public String EndOrder { get; set; }
    }
}
