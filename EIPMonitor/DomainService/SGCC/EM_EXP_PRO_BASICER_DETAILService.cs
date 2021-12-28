using EIPMonitor.Databse;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.SGCC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.SGCC
{
    public sealed class EM_EXP_PRO_BASICER_DETAIL_DETAILService
    {
        private readonly string sqlText = "select * from EM_EXP_PRO_BASICER_DETAIL where STANDARDVERSION = :STANDARDVERSION and (:BeginOrder is null or :EndOrder is null or PRODUCTION_ORDER_ID between :BeginOrder and :EndOrder) and ( (:BeginDate is null or :EndDate is null) or UPLOAD_TIME between :BeginDate and :EndDate)";
        private readonly static GenericQueryService<EM_EXP_PRO_BASICER_DETAIL> genericQueryService = new GenericQueryService<EM_EXP_PRO_BASICER_DETAIL>();

        public Task<List<EM_EXP_PRO_BASICER_DETAIL>> GetEntries(EM_EXP_PRO_BASICER_DETAIL EM_EXP_PRO_BASICER_DETAIL)
        {
            var result = genericQueryService.GetEntries(sqlText, EM_EXP_PRO_BASICER_DETAIL);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EM_EXP_PRO_BASICER_DETAIL_DETAILService,Method:GetEntries",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{IocKernel.Get<UserStamp>().EmployeeId} {IocKernel.Get<UserStamp>().UserName}",
                ParameterJson = JsonConvert.SerializeObject(EM_EXP_PRO_BASICER_DETAIL),
                SqlClauseOrFunction = $"Clause:{sqlText}"
            });
            return result;
        }
    }
}
