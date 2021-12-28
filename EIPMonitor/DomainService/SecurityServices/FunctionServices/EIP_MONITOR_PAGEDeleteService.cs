using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.SecurityModel;
using EIPMonitor.Transaction.Interface.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.SecurityServices.FunctionServices
{
    public class EIP_MONITOR_PAGEDeleteService
    {
        private readonly IDeleteCommand deleteCommand;
        private readonly string searchSql;
        private readonly string deleteSql;
        public EIP_MONITOR_PAGEDeleteService()
        {
            deleteCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            searchSql = "select * from EIP_MONITOR_PAGE where PAGE_NAME = :PAGE_NAME";
            deleteSql = "delete from EIP_MONITOR_PAGE where PAGE_NAME = :PAGE_NAME";
        }

        public async Task<EIP_MONITOR_PAGE> Delete(EIP_MONITOR_PAGE eIP_MONITOR_PAGE, UserStamp userStamp)
        {
            var createResult = await deleteCommand.Delete<EIP_MONITOR_PAGE>(deleteSql, eIP_MONITOR_PAGE).ConfigureAwait(false);
            if (createResult <= 0) return null;
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EIP_MONITOR_PAGECreateService,Method:Delete",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{userStamp.EmployeeId} {userStamp.UserName}",
                ParameterJson = JsonConvert.SerializeObject(eIP_MONITOR_PAGE),
                SqlClauseOrFunction = $"Search Clause:{searchSql}, Delete Clause:{deleteSql}"
            });
            return eIP_MONITOR_PAGE;
        }
    }
}
