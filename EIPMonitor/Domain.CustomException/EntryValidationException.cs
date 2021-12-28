using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Domain.CustomException
{
    public sealed class EntryValidationException:Exception
    {
        public EntryValidationException(string msg) : base(msg) { }
    }
}
