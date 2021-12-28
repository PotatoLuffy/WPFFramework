using Infrastructure.Standard.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Database.Interface.Generic
{
    public interface IScan
    {
        Task<List<T>> Scan<T>(Paging paging,string sqlText);
    }
}
