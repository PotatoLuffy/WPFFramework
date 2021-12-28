using Dapper;
using Infrastructure.Standard.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Database.Interface.Generic
{
    public interface ISearch: IExtractEntryWithoutTracking
    {
        Task<List<T>> Search<T>(DynamicParameters parameters, string sqlText);
    }
}
