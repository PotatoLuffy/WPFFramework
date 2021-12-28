﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.SGCC
{
    public class PP_TEST_BATCUR
    {
        //        PCB_CODE VARCHAR2			32
        //DEVICE_NO VARCHAR2			50
        public DateTime UPLOAD_TIME { get; set; }
        //UPLOAD_TIME DATE			7
        //FLAG NUMBER			22
        //PUTTIME DATE			7
        public Int32 STANDARDVERSION { get; set; }
        //STANDARDVERSION NUMBER			22
        public String PRODUCTION_ORDER_ID { get; set; }
        //PRODUCTION_ORDER_ID VARCHAR2			32
        //WORK_ORDER_CODE VARCHAR2			32
        public String TEST_CODE { get; set; }
        //TEST_CODE VARCHAR2			32
        //PLANT_CODE VARCHAR2			32
        //CHECK_TIME DATE			7
        //CREATE_TIME DATE			7
        //MODEL_CODE VARCHAR2			50
        //LOWER_VALUE NUMBER			22
        //UPPER_VALUE NUMBER			22
        //INFO_TYPE_CODE VARCHAR2			10
        public Decimal CURRENT_VALUE { get; set; }
        //CURRENT_VALUE NUMBER			22
        public Int32 CONCLUSION { get; set; }
        //CONCLUSION NUMBER			22
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String BeginOrder { get; set; }
        public String EndOrder { get; set; }
    }
}
