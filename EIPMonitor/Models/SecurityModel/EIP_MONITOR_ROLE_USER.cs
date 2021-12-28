using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.SecurityModel
{
    public class EIP_MONITOR_ROLE_USER
    {
        [StringLength(200)]
        public string ROLE_NAME { get; set; }
        [StringLength(200)]
        public string DEPARTMENT { get; set; }
        [StringLength(200)]
        public string EMPLOYEEID { get; set; }
    }
}
