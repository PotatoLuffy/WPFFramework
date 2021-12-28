using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Domain.CustomException
{
   public  class TheSecurityStampCanNotBeEmptyException:Exception
    {
        public TheSecurityStampCanNotBeEmptyException() { }
        public TheSecurityStampCanNotBeEmptyException(string msg) : base(msg) { }
    }
}
