using EIPMonitor.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Transaction.Interface.Generic
{
    interface IUpdateCommand:IUpdate,IExtractEntryWithoutTracking
    {
    }
}
