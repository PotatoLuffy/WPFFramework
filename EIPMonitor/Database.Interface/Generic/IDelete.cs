using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Database.Interface.Generic
{
    public interface IDelete
    {
        Task<Int32> Delete<T>(string sqlText, T @t);
    }
}
