using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.MasterData
{
    public class EIP_PRO_GlobalParamConfigure
    {
        [StringLength(100)]
        public String ParameterName { get; set; }
        [StringLength(1000)]
        public String Parameter { get; set; }
        [StringLength(500)]
        public String Description { get; set; }
        [StringLength(300)]
        public String ComputeBit { get; set; }
        [StringLength(100)]
        public String Creator { get; set; }
    }

    public enum EIP_PRO_GlobalParameter
    {
        Software_Version = 0,
        EIP_StandardVersion = 1,
        SGCCPROD = 2,
        SGCCQAS = 3,
        em_exp_pro_paramset_test_name = 4,
        em_exp_pro_paramset_Info_type_code = 5,
        Current_SGCCConnection = 6,
        EIP_CAL_MO_SCORE_CurrentVersion = 7,
        MES_TestDb=8,
        MESStandardDB=9,
        Eip_Min_Data_Create_and_Get=10,
        Synchronous_Black_Range_Begin_Time = 11,
        Synchronous_Black_Range_End_Time  =12,
        Synchronous_Black_Range_Begin_Break_Time_Start=13,
        Synchronous_Black_Range_Begin_Break_Time_End =14


    }
}
