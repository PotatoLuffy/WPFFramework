using EIPMonitor.Database.Interface.Generic;
using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.MasterData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.MasterData
{
    public class EIP_PRO_GlobalParamConfigureService
    {
        private readonly ISearch search;
        private readonly String searchSql;
        public EIP_PRO_GlobalParamConfigureService()
        {
            search = new CRUDService(LocalConstant.oracleConnectionStringBuilder);
            this.searchSql = "select * from EIP_PRO_GlobalParamConfigure where PARAMETERNAME=:ParameterName";
        }

        public Task<EIP_PRO_GlobalParamConfigure> ExtractConfiguration(EIP_PRO_GlobalParameter eIP_PRO_GlobalParameter)
        {
            EIP_PRO_GlobalParamConfigure param = new EIP_PRO_GlobalParamConfigure() { ParameterName = eIP_PRO_GlobalParameter.ToString() };
            var result = search.ExtractEntry<EIP_PRO_GlobalParamConfigure>(searchSql, param);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EIP_PRO_GlobalParamConfigureService,Method:ExtractConfiguration",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{IocKernel.Get<UserStamp>().EmployeeId} {IocKernel.Get<UserStamp>().UserName}",
                ParameterJson = JsonConvert.SerializeObject(eIP_PRO_GlobalParameter),
                SqlClauseOrFunction = $"Search Clause:{searchSql}"
            });
            return result;
        }
    }
}
