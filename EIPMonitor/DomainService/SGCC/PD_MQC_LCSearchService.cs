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
    public class PD_MQC_LCSearchService
    {
        private readonly string sqlText = "select * from PD_MQC_LC where STANDARDVERSION = :STANDARDVERSION and  (:BeginOrder is null or :EndOrder is null or PRODUCTION_ORDER_ID between :BeginOrder and :EndOrder) and ( (:BeginDate is null or :EndDate is null) or UPLOAD_TIME between :BeginDate and :EndDate)";
        private readonly static GenericQueryService<PD_MQC_LC> genericQueryService = new GenericQueryService<PD_MQC_LC>();

        public Task<List<PD_MQC_LC>> GetEntries(PD_MQC_LC eM_EXP_PRO_BASICER)
        {
            var result = genericQueryService.GetEntries(sqlText, eM_EXP_PRO_BASICER);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:PD_MQC_LCSearchService,Method:GetEntries",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{IocKernel.Get<UserStamp>().EmployeeId} {IocKernel.Get<UserStamp>().UserName}",
                ParameterJson = JsonConvert.SerializeObject(eM_EXP_PRO_BASICER),
                SqlClauseOrFunction = $"Clause:{sqlText}"
            });
            return result;
        }
    }
}
