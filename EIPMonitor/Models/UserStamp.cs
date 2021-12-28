using EIPMonitor.LocalInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model
{
    public class UserStamp : IUserStamp
    {
        public string UserName { get; set; }
        public string EmployeeId { get; set; }
        public string Address { get => LocalConstant.GetLocalIPAddress(); }

        public void Assign(IUserStamp source)
        {
            if (source == null) throw new NullReferenceException("未赋值");
            if (!source.IsAvailable()) throw new ArgumentException("不可给空对象");
            this.UserName = source.UserName;
            this.EmployeeId = source.EmployeeId;
        }
        public bool IsAvailable()
        {
            return !String.IsNullOrEmpty(this.UserName) && !String.IsNullOrEmpty(this.EmployeeId);
        }
    }
}
