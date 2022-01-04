using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Domain.CustomException
{
    public class OAResponseFailureException:Exception
    {
        public OAResponseFailureException() { }
        public OAResponseFailureException(string msg) : base(msg) { }
    }
}
