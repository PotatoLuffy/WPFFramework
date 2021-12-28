using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Domain.CustomException
{
    public class ThePasswordCanNotBeEmptyException : Exception
    {
        public ThePasswordCanNotBeEmptyException() { }
        public ThePasswordCanNotBeEmptyException(string msg) : base(msg) { }
    }
}
