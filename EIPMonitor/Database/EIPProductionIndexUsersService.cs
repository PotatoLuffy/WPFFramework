using Dapper;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System;

namespace EIPMonitor.Databse
{
    public class EIPProductionIndexUsersService
    {
        private OracleConnectionStringBuilder oracleConn;
        public EIPProductionIndexUsersService()
        {
            oracleConn = LocalConstant.oracleConnectionStringBuilder;
        }
        public async Task<EIPProductionIndexUsers> GetEntry(string sqlText, EIPProductionIndexUsers eIPProductionIndexUsers)
        {
            using (IDbConnection conn = new OracleConnection(oracleConn.ToString()))
            {
                var result = await conn.QueryFirstOrDefaultAsync<EIPProductionIndexUsers>(sqlText, eIPProductionIndexUsers).ConfigureAwait(false);
                return result;
            }
        }
    }
}
