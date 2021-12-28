using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Domain.CustomException
{
    public sealed class DuplicatedEntryException:Exception
    {
        public DuplicatedEntryException(string msg) : base(msg) { }
    }
}
