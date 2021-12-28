using EIPMonitor.LocalInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model
{
    public class UserStamp
    {
        public string UserName { get; set; }
        public string EmployeeId { get; set; }
        public string Address { get => LocalConstant.GetLocalIPAddress(); }
    }
}
