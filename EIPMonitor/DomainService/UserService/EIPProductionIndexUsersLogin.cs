using EIPMonitor.Database.Interface.Generic;
using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.UserService
{
   public sealed class EIPProductionIndexUsersLogin
    {
        private readonly ISearch search;
        private readonly string sqlText;
        public EIPProductionIndexUsersLogin()
        {
            this.search = new CRUDService(LocalConstant.oracleConnectionStringBuilder);
            this.sqlText = "select * from EIPProductionIndexUsers where (UserName = :UserName or EmployeeId = :UserName) and Status = :Status";
        }

        public async Task<EIPProductionIndexUsers> Login(EIPProductionIndexUsers eIPProductionIndexUsers)
        {
            eIPProductionIndexUsers.Status = Model.Widget.Status.Active;
            eIPProductionIndexUsers.StatusName = Model.Widget.Status.Active.ToString();
            var entry = await search.ExtractEntry<EIPProductionIndexUsers>(sqlText, eIPProductionIndexUsers).ConfigureAwait(false);
                EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
                {
                    FunctionCalledInLogical = "DomainService:EIPProductionIndexUsersChangeStatusService,Method:ChangeStatus",
                    OperateDateTime = DateTime.Now,
                    OperatorUser = $"{entry?.EmployeeId?? eIPProductionIndexUsers.UserName} {entry?.UserName?? eIPProductionIndexUsers.UserName}",
                    ParameterJson = JsonConvert.SerializeObject(eIPProductionIndexUsers),
                    SqlClauseOrFunction = $"searchSql Clause:{sqlText}"
                });
            if (entry == null) return null;
            if (entry.Password.Equals(eIPProductionIndexUsers.Password) && entry.Password.Equals("123")) return entry;
            var recomputeThePassword = UserEncryptionService.Encrypt(entry, eIPProductionIndexUsers.Password);
            if (recomputeThePassword.Equals(entry.Password)) return entry;
            
            return null;
        }
    }
}
