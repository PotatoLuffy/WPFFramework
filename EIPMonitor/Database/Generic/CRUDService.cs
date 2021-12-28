using Dapper;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Transaction.Interface.Generic;
using Infrastructure.Standard.Type;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Databse.Generic
{
    public sealed class CRUDService:ICreateCommand,IUpdateCommand, IExtractCommand, IDeleteCommand
    {
        private OracleConnectionStringBuilder oracleConn;
        public CRUDService(OracleConnectionStringBuilder oracleConn)
        {
            this.oracleConn = oracleConn;
        }
        public async Task<Int32> Create<T>(string sqlText, T @t)
        {
            using (IDbConnection conn = new OracleConnection(oracleConn.ToString()))
            {
                var result = await conn.ExecuteAsync(sqlText, @t).ConfigureAwait(false);
                return result;
            }
        }
        public async Task<T> ExtractEntry<T>(String sqlText, T @t)
        {
            using (IDbConnection conn = new OracleConnection(oracleConn.ToString()))
            {
                var result = await conn.QueryFirstOrDefaultAsync<T>(sqlText, @t).ConfigureAwait(false);
                return result;
            }
        }
        public async Task<Int32> Update<T>(string sqlText, T @t)
        {
            using (IDbConnection conn = new OracleConnection(oracleConn.ToString()))
            {
                var result = await conn.ExecuteAsync(sqlText, @t).ConfigureAwait(false);
                return result;
            }
        }
        public async Task<Int32> Delete<T>(string sqlText, T @t)
        {
            using (IDbConnection conn = new OracleConnection(oracleConn.ToString()))
            {
                var result = await conn.ExecuteAsync(sqlText, @t).ConfigureAwait(false);
                return result;
            }
        }
        public async Task<List<T>> Search<T>(DynamicParameters parameters, string sqlText)
        {
            using (IDbConnection conn = new OracleConnection(oracleConn.ToString()))
            {
                var result = await conn.QueryAsync<T>(sqlText, parameters).ConfigureAwait(false);
                return result.ToList();
            }
        }

        public async Task<List<T>> Scan<T>(Paging paging, string sqlText)
        {
            using (IDbConnection conn = new OracleConnection(oracleConn.ToString()))
            {
                var result = await conn.QueryAsync<T>(sqlText, paging).ConfigureAwait(false);
                return result.ToList();
            }
        }

    }
}
