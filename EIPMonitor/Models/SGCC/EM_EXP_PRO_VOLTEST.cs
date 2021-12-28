using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.SGCC
{
    public class EM_EXP_PRO_VOLTEST
    {
        //        FLAG NUMBER			22	1
        //PUTTIME DATE			7	
        //TEST_NAME VARCHAR2			50	
        //PCB_CODE VARCHAR2			32	
        //DEVICE_NO VARCHAR2			32	
        //LEAKAGE_CURRENT NUMBER			22	10
        public Decimal LEAKAGE_CURRENT { get; set; }
        public DateTime UPLOAD_TIME { get; set; }
        //UPLOAD_TIME DATE			7	
        public Int32 STANDARDVERSION { get; set; }
        //STANDARDVERSION NUMBER			22	11
        public String PRODUCTION_ORDER_ID { get; set; }
        //PRODUCTION_ORDER_ID VARCHAR2			32	
        //WORK_ORDER_CODE VARCHAR2			32	
        public String TEST_CODE { get; set; }
        //TEST_CODE VARCHAR2			32	
        //PLANT_CODE VARCHAR2			32	
        //CHECK_TIME DATE			7	
        //CREATE_TIME DATE			7	
        //MODEL_CODE VARCHAR2			50	
        //INFO_TYPE_CODE VARCHAR2			10	
        //DEVICE_CODE VARCHAR2			50	
        //EPITOP_CODE VARCHAR2			50	
        //CHECK_DURATION VARCHAR2			10	
        //TESTING_VOLTAGE VARCHAR2			10	
        //UNQUALIFIED VARCHAR2			10	
        public Int32 CONCLUSION { get; set; }
        //CONCLUSION NUMBER			22	11
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String BeginOrder { get; set; }
        public String EndOrder { get; set; }
    }
}
