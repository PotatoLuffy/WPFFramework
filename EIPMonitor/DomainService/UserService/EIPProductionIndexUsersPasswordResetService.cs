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
    public sealed class EIPProductionIndexUsersPasswordResetService
    {
        private readonly IUpdateCommand updateCommand;
        private readonly string searchSql;
        private readonly string updateSql;
        public EIPProductionIndexUsersPasswordResetService()
        {
            updateCommand = new CRUDService(LocalConstant.oracleConnectionStringBuilder);
            searchSql = "select * from EIPProductionIndexUsers where EmployeeId = :EmployeeId";
            updateSql = "update EIPProductionIndexUsers set Password = :Password where EmployeeId = :EmployeeId";

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eIPProductionIndexUsers"></param>
        /// <returns></returns>
        /// <exception cref="EIPMonitor.Domain.CustomException.EntryValidationException">Validation the parameter</exception>
        /// <exception cref="EIPMonitor.Domain.CustomException.EntryNotFoundException">Entry not found.</exception>
        public async Task<EIPProductionIndexUsers> PasswordReset(EIPProductionIndexUsers eIPProductionIndexUsers)
        {
            var dbEntry = await updateCommand.ExtractEntry<EIPProductionIndexUsers>(searchSql, eIPProductionIndexUsers).ConfigureAwait(false);
            if (dbEntry == null) throw new EntryNotFoundException("重置失败，未找到该账号。");
            //if (dbEntry.Password != eIPProductionIndexUsers.Password) throw new EntryValidationException("重置失败，旧密码错误。");
            ValidateNewPassword(eIPProductionIndexUsers);
            dbEntry.Password = eIPProductionIndexUsers.newPassword;
            var result = await updateCommand.Update<EIPProductionIndexUsers>(updateSql, eIPProductionIndexUsers).ConfigureAwait(false);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EIPProductionIndexUsersPasswordResetService,Method:EIPProductionIndexUsersPasswordResetService",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{IocKernel.Get<UserStamp>().EmployeeId} {IocKernel.Get<UserStamp>().UserName}",
                ParameterJson = JsonConvert.SerializeObject(eIPProductionIndexUsers),
                SqlClauseOrFunction = $"searchSql Clause:{searchSql},update Claulse:{updateSql}"
            });

            if (result <= 0) return null;
            return dbEntry;
        }
        private void ValidateNewPassword(EIPProductionIndexUsers eIPProductionIndexUsers)
        {
            if (String.IsNullOrWhiteSpace(eIPProductionIndexUsers.newPassword)) throw new EntryValidationException("新密码无效。");
        }
    }
}
