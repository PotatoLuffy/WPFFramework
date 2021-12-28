using EIPMonitor.Databse;
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
    public class PP_TEST_FCTSearchService
    {
        private readonly string sqlText = "select * from PP_TEST_FCT where STANDARDVERSION = :STANDARDVERSION and  (:BeginOrder is null or :EndOrder is null or PRODUCTION_ORDER_ID between :BeginOrder and :EndOrder)  and ( (:BeginDate is null or :EndDate is null) or UPLOAD_TIME between :BeginDate and :EndDate)";
        private readonly static GenericQueryService<PP_TEST_FCT> genericQueryService = new GenericQueryService<PP_TEST_FCT>();
        public Task<List<PP_TEST_FCT>> GetEntries(PP_TEST_FCT eM_EXP_PRO_BASICER)
        {
            var result = genericQueryService.GetEntries(sqlText, eM_EXP_PRO_BASICER);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:PP_TEST_BATCURSearchService,Method:GetEntries",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{LoginWindow.UserStamp.EmployeeId} {LoginWindow.UserStamp.UserName}",
                ParameterJson = JsonConvert.SerializeObject(eM_EXP_PRO_BASICER),
                SqlClauseOrFunction = $"Clause:{sqlText}"
            });
            return result;
        }
    }
}
