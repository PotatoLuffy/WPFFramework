using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Transaction.Interface.Generic;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.OracleProcedureService
{
    public class ProcedureGenericService
    {
        private ICreateCommand createCommand;
        public ProcedureGenericService(OracleConnectionStringBuilder oracleConnectionStringBuilder)
        {
            createCommand = new CRUDService(oracleConnectionStringBuilder);
        }

        public Task<Int32> CallProcedure<T>(string sqlText, T @t)
        {
            var result = createCommand.Create<T>(sqlText, @t);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:ProcedureGenericService,Method:CallProcedure",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{IocKernel.Get<UserStamp>().EmployeeId} {IocKernel.Get<UserStamp>().UserName}",
                ParameterJson = JsonConvert.SerializeObject(@t),
                SqlClauseOrFunction = $"Clause:{sqlText}"
            });
            return result;
        }
    }
}
