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
    public class MOSearchService
    {
        private IExtractCommand extractCommand;
        private readonly string searchEntry;
        public MOSearchService()
        {
            extractCommand = new MESCURDService(LocalConstant.CurrentMESDBConnection);
            searchEntry = "select * from Mo where MOName = @MOName";
        }

        public Task<MO> GetEntry(MO mo)
        {
            var result = extractCommand.ExtractEntry<MO>(searchEntry, mo);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:MOSearchService,Method:GetEntry",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{LoginWindow.UserStamp.EmployeeId} {LoginWindow.UserStamp.UserName}",
                ParameterJson = JsonConvert.SerializeObject(mo),
                SqlClauseOrFunction = $"Search Clause:{searchEntry}"
            });
            return result;
        }
    }
}
