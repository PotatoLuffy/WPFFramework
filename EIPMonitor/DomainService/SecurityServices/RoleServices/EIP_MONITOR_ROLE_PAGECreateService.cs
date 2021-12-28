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

namespace EIPMonitor.DomainServices.SecurityServices.RoleServices
{
    public class EIP_MONITOR_ROLE_PAGECreateService
    {
        private ICreateCommand createCommand;
        private String searchSql;
        private String createSql;
        public EIP_MONITOR_ROLE_PAGECreateService()
        {
            createCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            searchSql = "select * from EIP_MONITOR_ROLE_PAGE where PAGE_NAME = :PAGE_NAME and ROLE_NAME = :ROLE_NAME and DEPARTMENT = :DEPARTMENT ";
            createSql = "insert into EIP_MONITOR_ROLE_PAGE ( PAGE_NAME,ROLE_NAME,DEPARTMENT,RIGHTCODE,RIGHTNAME ) values( :PAGE_NAME, :ROLE_NAME, :DEPARTMENT, :RIGHTCODE, :RIGHTNAME ) ";
        }

        public async Task<EIP_MONITOR_ROLE_PAGE> Create(EIP_MONITOR_ROLE_PAGE eIP_MONITOR_ROLE_PAGE, UserStamp userStamp)
        {
            var existedEntry = await createCommand.ExtractEntry<EIP_MONITOR_ROLE_PAGE>(searchSql, eIP_MONITOR_ROLE_PAGE).ConfigureAwait(false);
            if (existedEntry != null) return null;
            var result = await createCommand.Create<EIP_MONITOR_ROLE_PAGE>(createSql, eIP_MONITOR_ROLE_PAGE).ConfigureAwait(false);
            if (result <= 0) return null;
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EIP_MONITOR_ROLE_PAGECreateService,Method:Create",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{userStamp.EmployeeId} {userStamp.UserName}",
                ParameterJson = JsonConvert.SerializeObject(eIP_MONITOR_ROLE_PAGE),
                SqlClauseOrFunction = $"Search Clause:{searchSql}, Create Clause:{createSql}"
            }) ;
            return eIP_MONITOR_ROLE_PAGE;
        }
    }
}
