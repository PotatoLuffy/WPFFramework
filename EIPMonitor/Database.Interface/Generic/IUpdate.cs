using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Database.Interface
{
    public interface IUpdate
    {
        Task<Int32> Update<T>(String sqlText, T @t);
    }
}
