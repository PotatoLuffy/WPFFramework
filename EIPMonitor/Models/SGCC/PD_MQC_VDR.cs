using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.SGCC
{
    public class PD_MQC_VDR
    {
        public String TEST_CODE { get; set; }
        //        TEST_CODE VARCHAR2			32
        public Int32 STANDARDVERSION { get; set; }
        //STANDARDVERSION NUMBER			22
        public String PRODUCTION_ORDER_ID { get; set; }
        //PRODUCTION_ORDER_ID VARCHAR2			32
        //WORK_ORDER_CODE VARCHAR2			32
        //CHECK_TIME DATE			7
        //CREATE_TIME DATE			7
        //MATERIAL_SUPPLIER VARCHAR2			150
        //MODEL_CODE VARCHAR2			150
        //LOWER_VALUE VARCHAR2			50
        //UPPER_VALUE VARCHAR2			50
        //INFO_TYPE_CODE VARCHAR2			10
        //VARISTOR_VOLTAGE NUMBER			22
        public Int32 CONCLUSION { get; set; }
        //CONCLUSION NUMBER			22
        //MATERIAL_CHECK_BATCH VARCHAR2			50
        //MATERIAL_BATCH VARCHAR2			50
        //MATERIAL_BRAND VARCHAR2			100
        //MATERIAL_LEAVE_TIME DATE			7
        //INCOMING_CHECK_TIME DATE			7
        public DateTime UPLOAD_TIME { get; set; }
        //UPLOAD_TIME DATE			7
        public Decimal ERROR_VALUE { get; set; }
        //ERROR_VALUE NUMBER			22
        //FLAG NUMBER			22
        //PUTTIME DATE			7
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String BeginOrder { get; set; }
        public String EndOrder { get; set; }
    }
}
