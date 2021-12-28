using Dapper;
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
    public class EIP_MONITOR_PAGECreateService
    {
        private readonly ICreateCommand createCommand;
        private readonly IUpdateCommand updateCommand;
        private readonly string searchSql;
        private readonly string updateSql;
        private readonly string insertSql;
        public EIP_MONITOR_PAGECreateService()
        {
            createCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            updateCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            searchSql = "select * from EIP_MONITOR_PAGE where PAGE_NAME = :PAGE_NAME";
            updateSql = "update EIP_MONITOR_PAGE set PAGE_FUNCTION_NAME = :PAGE_FUNCTION_NAME,OrderWeight = :OrderWeight,STATUS = STATUS, STATUS_NAME = :STATUS_NAME where PAGE_NAME = :PAGE_NAME";
            insertSql = "insert into EIP_MONITOR_PAGE( PAGE_NAME,PAGE_FUNCTION_NAME,OrderWeight,STATUS,STATUS_NAME) values(:PAGE_NAME,:PAGE_FUNCTION_NAME,:OrderWeight,:STATUS,:STATUS_NAME)";
        }

        public async Task<EIP_MONITOR_PAGE> CreateOrUpdate(EIP_MONITOR_PAGE eIP_MONITOR_PAGE, UserStamp userStamp)
        {
            var result = await createCommand.ExtractEntry<EIP_MONITOR_PAGE>(searchSql, eIP_MONITOR_PAGE).ConfigureAwait(false);
            if (result != null)
            {
                var updateResult = await updateCommand.Update<EIP_MONITOR_PAGE>(updateSql, eIP_MONITOR_PAGE).ConfigureAwait(true) ;
                if (updateResult <= 0) return null;
                EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
                {
                    FunctionCalledInLogical = "DomainService:EIP_MONITOR_PAGECreateService,Method:CreateOrUpdate",
                    OperateDateTime = DateTime.Now,
                    OperatorUser = $"{userStamp.EmployeeId} {userStamp.UserName}",
                    ParameterJson = JsonConvert.SerializeObject(eIP_MONITOR_PAGE),
                    SqlClauseOrFunction = $"Search Clause:{searchSql}, Update Clause:{updateSql}"
                });
                return eIP_MONITOR_PAGE;
            }
            var createResult = await createCommand.Create<EIP_MONITOR_PAGE>(insertSql, eIP_MONITOR_PAGE).ConfigureAwait(false);
            if (createResult <= 0) return null;
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EIP_MONITOR_PAGECreateService,Method:CreateOrUpdate",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{userStamp.EmployeeId} {userStamp.UserName}",
                ParameterJson = JsonConvert.SerializeObject(eIP_MONITOR_PAGE),
                SqlClauseOrFunction = $"Search Clause:{searchSql}, Update Clause:{insertSql}"
            });
            return eIP_MONITOR_PAGE;
        }
    }
}
