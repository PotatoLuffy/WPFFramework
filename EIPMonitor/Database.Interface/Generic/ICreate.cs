using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Database.Interface
{
    public interface ICreate
    {
        Task<Int32> Create<T>(string sqlText, T @t);
    }
}
