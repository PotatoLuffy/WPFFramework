using Dapper;
using EIPMonitor.Transaction.Interface.Generic;
using Infrastructure.Standard.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Databse.Generic
{
    public class MESCURDService: IExtractCommand
    {
        private SqlConnectionStringBuilder sqlConn;
        public MESCURDService(SqlConnectionStringBuilder sqlConn)
        {
            this.sqlConn = sqlConn;
        }
        public async Task<List<T>> Search<T>(DynamicParameters parameters, string sqlText)
        {
            using (IDbConnection conn = new SqlConnection(sqlConn.ToString()))
            {
                var result = await conn.QueryAsync<T>(sqlText, parameters).ConfigureAwait(false);
                return result.ToList();
            }
        }

        public async Task<List<T>> Scan<T>(Paging paging, string sqlText)
        {
            using (IDbConnection conn = new SqlConnection(sqlConn.ToString()))
            {
                var result = await conn.QueryAsync<T>(sqlText, paging).ConfigureAwait(false);
                return result.ToList();
            }
        }

        public async Task<T> ExtractEntry<T>(String sqlText, T @t)
        {
            using (IDbConnection conn = new SqlConnection(sqlConn.ToString()))
            {
                var result = await conn.QueryFirstOrDefaultAsync<T>(sqlText, @t).ConfigureAwait(false);
                return result;
            }
        }
    }
}
