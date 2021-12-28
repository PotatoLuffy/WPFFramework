using Dapper;
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
    public sealed class EIPProductionIndexUsersSearchService
    {
        private readonly IExtractCommand extractCommand;
        private string sqlText;
        public EIPProductionIndexUsersSearchService()
        {
            this.extractCommand = new CRUDService(LocalConstant.oracleConnectionStringBuilder);
            this.sqlText = "select * from EIPProductionIndexUsers where rownum between :OffSetNum and :EndNum";
        }
        public Task<List<EIPProductionIndexUsers>> Search(Paging paging, EIPProductionIndexUsers eIPProductionIndexUsers)
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("OffSetNum", paging.OffSetNum);
            dynamicParameter.Add("EndNum", paging.OffSetNum + paging.Rows);
            dynamicParameter.Add("EmployeeId", eIPProductionIndexUsers.EmployeeId);
            var result = extractCommand.Search<EIPProductionIndexUsers>(dynamicParameter, sqlText);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EIPProductionIndexUsersSearchService,Method:Search",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{IocKernel.Get<UserStamp>().EmployeeId} {IocKernel.Get<UserStamp>().UserName}",
                ParameterJson = JsonConvert.SerializeObject(dynamicParameter),
                SqlClauseOrFunction = $"searchSql Clause:{sqlText}"
            });

            return result;
        }
    }
}
