using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Database.Interface
{
    public interface IExtractEntryWithoutTracking
    {
        Task<T> ExtractEntry<T>(String sqlText, T @t);
    }
}
