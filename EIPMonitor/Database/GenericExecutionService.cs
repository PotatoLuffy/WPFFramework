using EIPMonitor.LocalInfrastructure;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using EIPMonitor.Model.Widget;

namespace EIPMonitor.Databse
{
    public class GenericExecutionService<T>
    {
        public async Task<Int32> TestExecuteAsync(string sqlText, T @t)
        {
            using (IDbConnection conn = new SqlConnection(LocalConstant.CurrentMESDBConnection.ToString()))
            {
                
                var result = await conn.ExecuteAsync(sqlText, @t,commandTimeout:3600).ConfigureAwait(true);
                return result;
            }
        }
        public async Task<Int32> TestExecuteAsync(string sqlText, DynamicParameters @t)
        {
            using (IDbConnection conn = new SqlConnection(LocalConstant.CurrentMESDBConnection.ToString()))
            {

                var result = await conn.QueryAsync<Int32>(sqlText, @t, commandTimeout: 3600).ConfigureAwait(true);
                return result.FirstOrDefault();
            }
        }
        public async Task<Int32> SGCCExecuteAsync(string sqlText, DynamicParameters @t)
        {
            using (IDbConnection conn = new OracleConnection(LocalConstant.OracleCurrentConnectionStringBuilder.ToString()))
            {

                var result = await conn.QueryAsync<Int32>(sqlText, @t, commandTimeout: 3600).ConfigureAwait(true);
                return result.FirstOrDefault();
            }
        }
        public async Task<T> SGCCQueryAsync(string sqlText, T @t)
        {
            using (IDbConnection conn = new OracleConnection(LocalConstant.OracleCurrentConnectionStringBuilder.ToString()))
            {
                var result = await conn.QueryFirstOrDefaultAsync<T>(sqlText, @t).ConfigureAwait(false);
                return result;
            }
        }

        public async Task<List<T>> SGCCExecuteAndQueryAsync(string sqlText, OracleDynamicParameters @t)
        {
            using (IDbConnection conn = new OracleConnection(LocalConstant.OracleCurrentConnectionStringBuilder.ToString()))
            {
                var result = await conn.QueryAsync<T>(sqlText,@t,commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return result.ToList();
            }
        }
    }
}
