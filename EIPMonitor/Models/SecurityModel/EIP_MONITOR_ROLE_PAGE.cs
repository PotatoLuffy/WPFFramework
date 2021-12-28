using EIPMonitor.Model.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.SecurityModel
{
    public class EIP_MONITOR_ROLE_PAGE
    {
        public String PAGE_NAME { get; set; }
        public String ROLE_NAME { get; set; }
        public String DEPARTMENT { get; set; }
        public RightEnum RIGHTCODE { get; set; }
        public String RIGHTNAME { get; set; }
    }
}
