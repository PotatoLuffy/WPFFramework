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
    public sealed class EM_EXP_PRO_BASICERSearchServices
    {
        private readonly string sqlText = "select * from EM_EXP_PRO_BASICER where STANDARDVERSION = :STANDARDVERSION and (:BeginOrder is null or :EndOrder is null or PRODUCTION_ORDER_ID between :BeginOrder and :EndOrder) and ( (:BeginDate is null or :EndDate is null) or UPLOAD_TIME between :BeginDate and :EndDate)";
        private readonly static GenericQueryService<EM_EXP_PRO_BASICER> genericQueryService = new GenericQueryService<EM_EXP_PRO_BASICER>();

        public Task<List<EM_EXP_PRO_BASICER>> GetEntries(EM_EXP_PRO_BASICER eM_EXP_PRO_BASICER)
        {
            var result = genericQueryService.GetEntries(sqlText, eM_EXP_PRO_BASICER);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EM_EXP_PRO_BASICER_DETAIL_DETAILService,Method:GetEntries",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{IocKernel.Get<UserStamp>().EmployeeId} {IocKernel.Get<UserStamp>().UserName}",
                ParameterJson = JsonConvert.SerializeObject(eM_EXP_PRO_BASICER),
                SqlClauseOrFunction = $"Clause:{sqlText}"
            });
            return result;
        }
    }
}
