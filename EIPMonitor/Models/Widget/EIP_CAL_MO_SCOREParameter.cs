using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.Widget
{
   public  class EIP_CAL_MO_SCOREParameter
    {
        public PI_Method pI_Method { get; set; }
        public String PI_Mo { get; set; }
    }

    public enum PI_Method
    {
        Calculate_Those_MOs_Which_HasnT_Calculated = 0,
        Calculate_Specific_Mo_Score = 1,
        Recalculate_All_The_Mos = 88
    }
}
