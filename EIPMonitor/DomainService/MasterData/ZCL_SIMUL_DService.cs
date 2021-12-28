using Dapper;
using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.MasterData;
using EIPMonitor.Transaction.Interface.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.MasterData
{
    public class ZCL_SIMUL_DService
    {
        private readonly IExtractCommand extractCommand;
        private  string rangeSearch;
        private  string listSearch;
        public ZCL_SIMUL_DService()
        {
            extractCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            rangeSearch = " select a.*,b.ZTYPE,b.ZDESC,b.ZFLAG,b.ZVALID,b.ZDSCORE,b.ZVER,b.ZWEIGHT,c.MATERIALSNAME from zcl_simul_d a inner join zcl_type b on a.SUBCLASSCODE = b.SUBCLASSCODE and a.ZKIND = b.ZKIND "
                + " left outer join  (select IPONO, max( MATERIALSNAME) MATERIALSNAME FROM ZGW_MO_T  GROUP BY IPONO)  c on c.IPONO = a.WORK_ORDER_CODE"
+ " where a.WORK_ORDER_CODE between :beginOrder and :endOrder and b.ZVALID = 1 and a.WORK_ORDER_CODE like '{0}%'"
+ " order by WORK_ORDER_CODE ";

            listSearch = " select a.*,b.ZTYPE,b.ZDESC,b.ZFLAG,b.ZVALID,b.ZDSCORE,b.ZVER,b.ZWEIGHT,c.MATERIALSNAME from zcl_simul_d a inner join zcl_type b on a.SUBCLASSCODE = b.SUBCLASSCODE and a.ZKIND = b.ZKIND "
                                + " left outer join  (select IPONO, max( MATERIALSNAME) MATERIALSNAME FROM ZGW_MO_T  GROUP BY IPONO)  c on c.IPONO = a.WORK_ORDER_CODE"
+ " where a.WORK_ORDER_CODE in ({0}) and b.ZVALID = 1 and a.WORK_ORDER_CODE like '{1}%' "
+ " order by WORK_ORDER_CODE ";
        }
        public async Task<List<ZCL_SIMUL_D>> GetEntries(String beginOrder, String endOrder,List<String> workorderList,char startLetter)
        {
            if (string.IsNullOrWhiteSpace(beginOrder) && string.IsNullOrWhiteSpace(endOrder) && workorderList.Count <= 0) return null;
            string sqlText;
            DynamicParameters dynamicParameters = new DynamicParameters();
            //dynamicParameters.Add("startLetter", startLetter);
            if ((workorderList?.Count??0) <=0)
            {
                sqlText = String.Format(rangeSearch,startLetter);
                dynamicParameters.Add("beginOrder", beginOrder);
                dynamicParameters.Add("endOrder", endOrder);
                
                EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
                {
                    FunctionCalledInLogical = "DomainService:ZCL_SIMUL_DService,Method:GetEntries, Range Search",
                    OperateDateTime = DateTime.Now,
                    OperatorUser = $"{LoginWindow.UserStamp.EmployeeId} {LoginWindow.UserStamp.UserName}",
                    ParameterJson = JsonConvert.SerializeObject(dynamicParameters),
                    SqlClauseOrFunction = $"Search Clause:{sqlText}"
                });
            }
            else
            {
                String paramConcat = "";
                for (int inx = 0; inx < workorderList.Count; inx++)
                {
                    var paramName = $"param{inx}";
                    dynamicParameters.Add(paramName, workorderList[inx]);
                    paramConcat += $":{paramName},";

                }
                sqlText = String.Format(listSearch,paramConcat.TrimEnd(','),startLetter);
                EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
                {
                    FunctionCalledInLogical = "DomainService:ZCL_SIMUL_DService,Method:GetEntries, list Search",
                    OperateDateTime = DateTime.Now,
                    OperatorUser = $"{LoginWindow.UserStamp.EmployeeId} {LoginWindow.UserStamp.UserName}",
                    ParameterJson = JsonConvert.SerializeObject(dynamicParameters),
                    SqlClauseOrFunction = $"Search Clause:{sqlText}"
                });
            }
           var result = await extractCommand.Search<ZCL_SIMUL_D>(dynamicParameters, sqlText).ConfigureAwait(false);


           return result;

        }
    }
}
