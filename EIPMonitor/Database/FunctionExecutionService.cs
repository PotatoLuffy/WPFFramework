using Dapper;
using EIPMonitor.LocalInfrastructure;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Databse
{
    public class FunctionExecutionService
    {
        private OracleConnectionStringBuilder oracleConn;
        public FunctionExecutionService()
        {
            oracleConn = LocalConstant.oracleConnectionStringBuilder;
            
        }
        public async Task<String> ExecuteFunctionOrProcedure(string sqlText, DynamicParameters dynamicParameters)
        {
            using (IDbConnection conn = new OracleConnection(LocalConstant.oracleConnectionStringBuilderTest.ToString()))
            {
                var result = await conn.QueryAsync<String>(sqlText, dynamicParameters).ConfigureAwait(false);
                return result.FirstOrDefault();
            }
        }
    }
}
