using EIPMonitor.Databse.Generic;
using EIPMonitor.Domain.CustomException;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Transaction.Interface.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.UserService
{
    public sealed class EIPProductionIndexUsersChangeStatusService
    {
        private readonly IUpdateCommand updateCommand;
        private readonly string searchSql;
        private readonly string updateSql;
        public EIPProductionIndexUsersChangeStatusService()
        {
            this.updateCommand = new CRUDService(LocalConstant.oracleConnectionStringBuilder);
            searchSql = "select * from EIPProductionIndexUsers where EmployeeId = :EmployeeId";
            updateSql = "update EIPProductionIndexUsers set Status = :Status,StatusName = :StatusName where EmployeeId = :EmployeeId";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eIPProductionIndexUsers"></param>
        /// <returns></returns>
        /// <exception cref="EIPMonitor.Domain.CustomException.EntryNotFoundException">Not Found</exception>
        public async Task<EIPProductionIndexUsers> ChangeStatus(EIPProductionIndexUsers eIPProductionIndexUsers)
        {
            var dbEntry = await updateCommand.ExtractEntry<EIPProductionIndexUsers>(searchSql, eIPProductionIndexUsers);
            if (dbEntry == null) throw new EntryNotFoundException("未找到该记录");
            dbEntry.Status = dbEntry.Status == Model.Widget.Status.Active ? Model.Widget.Status.Inactive : Model.Widget.Status.Active;
            dbEntry.StatusName = dbEntry.Status.ToString();
            var result = await updateCommand.Update<EIPProductionIndexUsers>(updateSql, dbEntry);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EIPProductionIndexUsersChangeStatusService,Method:ChangeStatus",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{IocKernel.Get<UserStamp>().EmployeeId} {IocKernel.Get<UserStamp>().UserName}",
                ParameterJson = JsonConvert.SerializeObject(eIPProductionIndexUsers),
                SqlClauseOrFunction = $"searchSql Clause:{searchSql},update Claulse:{updateSql}"
            });
            if (result >= 0) return dbEntry;
            return null;

        }
    }
}
