using EIPMonitor.Model.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.SecurityModel
{
    public class EIP_MONITOR_PAGE
    {
        [StringLength(200)]
        public string PAGE_NAME { get; set; }
        [StringLength(200)]
        public string PAGE_FUNCTION_NAME { get; set; }
        public int OrderWeight { get; set; }
        public Status STATUS { get; set; }
        public String STATUS_NAME { get; set; }
    }
}
