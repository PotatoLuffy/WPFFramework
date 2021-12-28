using EIPMonitor.Databse.Generic;
using EIPMonitor.Domain.CustomException;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Transaction.Interface.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.UserService
{
    public class EIPProductionIndexUsersRegisterService
    {
        private ICreateCommand createCommand;
        private readonly string extractSqlText;
        private readonly string insertSqlText;
        public EIPProductionIndexUsersRegisterService()
        {
            createCommand = new CRUDService(LocalConstant.oracleConnectionStringBuilder);
            extractSqlText = "select * from EIPProductionIndexUsers where EmployeeId = :EmployeeId";
            insertSqlText = "insert into EIPProductionIndexUsers(UserName,Password,EmployeeId,Email,CreatedDate,Status,StatusName) values (:UserName,:Password,:EmployeeId,:Email,:CreatedDate,:Status,:StatusName)";
        }
        /// <summary>
        ///  Register New User
        /// </summary>
        /// <param name="eIPProductionIndexUsers"></param>
        /// <returns>return the entry created,or return null</returns>
        /// <exception cref="EIPMonitor.Domain.CustomException.EntryValidationException">The parameter validate failure</exception>
        /// <exception cref="EIPMonitor.Domain.CustomException.DuplicatedEntryException">The entry has existed in db already.</exception>
        public async Task<EIPProductionIndexUsers> Register(EIPProductionIndexUsers eIPProductionIndexUsers)
        {
            VerifyTheRegisterInfos(eIPProductionIndexUsers);
            var existdsEntry = await createCommand.ExtractEntry<EIPProductionIndexUsers>(extractSqlText, eIPProductionIndexUsers).ConfigureAwait(false);
            if (existdsEntry != null) throw new DuplicatedEntryException("该工号已注册");
            SetSomeStatus(eIPProductionIndexUsers);
            var result = await createCommand.Create<EIPProductionIndexUsers>(insertSqlText, eIPProductionIndexUsers).ConfigureAwait(false);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EIPProductionIndexUsersRegisterService,Method:Register",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{eIPProductionIndexUsers.EmployeeId} {eIPProductionIndexUsers.UserName}",
                ParameterJson = JsonConvert.SerializeObject(eIPProductionIndexUsers),
                SqlClauseOrFunction = $"searchSql Clause:{extractSqlText},Insert Claulse:{insertSqlText}"
            });

            if (result >= 1) return eIPProductionIndexUsers;
            return null;
        }
        private void SetSomeStatus(EIPProductionIndexUsers eIPProductionIndexUsers)
        {
            eIPProductionIndexUsers.Status = Model.Widget.Status.Inactive;
            eIPProductionIndexUsers.StatusName = eIPProductionIndexUsers.Status.ToString();
        }

        private void VerifyTheRegisterInfos(EIPProductionIndexUsers eIPProductionIndexUsers)
        {
            var ifAvailableEmployeeId = Int32.TryParse(eIPProductionIndexUsers.EmployeeId, out Int32 employeeId);
            if (!ifAvailableEmployeeId || employeeId <=0) throw new EntryValidationException("请输入正确工号");
            if (string.IsNullOrWhiteSpace(eIPProductionIndexUsers.Password)) throw new EntryValidationException("密码不可为空");
            try
            {
                var addr = new MailAddress(eIPProductionIndexUsers.Email);
                if(addr.Address != eIPProductionIndexUsers.Email) throw new EntryValidationException("请输入正确邮箱地址。");
            }
            catch (Exception e)
            {
                LocalConstant.Logger.Debug("AppDomainUnhandledExceptionHandler", e);
                throw new EntryValidationException("请输入正确邮箱地址。");
            }
            if (String.IsNullOrWhiteSpace(eIPProductionIndexUsers.UserName)) eIPProductionIndexUsers.UserName = "";

        }
    }
}
