using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Transaction.Interface.Generic;
using Infrastructure.Standard.Type;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.UserService
{
   public sealed class EIPProductionIndexUsersScanService
    {
        private readonly IExtractCommand extractCommand;
        private string sqlText;
        public EIPProductionIndexUsersScanService()
        {
            this.extractCommand = new CRUDService(LocalConstant.oracleConnectionStringBuilder);
            this.sqlText = "select * from EIPProductionIndexUsers where rownum between :OffSetNum and (:OffSetNum + :Rows)";
        }
        public  Task<List<EIPProductionIndexUsers>> Scan(Paging paging)
        {
            var result = extractCommand.Scan<EIPProductionIndexUsers>(paging, sqlText);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EIPProductionIndexUsersScanService,Method:Scan",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{LoginWindow.UserStamp.EmployeeId} {LoginWindow.UserStamp.UserName}",
                ParameterJson = JsonConvert.SerializeObject(paging),
                SqlClauseOrFunction = $"searchSql Clause:{sqlText}"
            });

            return result;
        }
    }
}
