using Dapper;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model.SGCC;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Databse
{
    public class GenericQueryService<T>
    {
        public async Task<List<T>> GetEntries(string sqlText, T @t)
        {
            using (IDbConnection conn = new OracleConnection(LocalConstant.OracleCurrentConnectionStringBuilder.ToString()))
            {
                var result = await conn.QueryAsync<T>(sqlText, @t).ConfigureAwait(false);
                return result.ToList();
            }
        }
    }
}
