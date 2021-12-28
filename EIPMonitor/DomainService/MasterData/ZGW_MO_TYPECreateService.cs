using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.SGCC_Master;
using EIPMonitor.Transaction.Interface.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.MasterData
{
    public class ZGW_MO_TYPECreateService
    {
        private ICreateCommand createCommand;
        private string searchSql;
        private string insertSql;
        public ZGW_MO_TYPECreateService()
        {
            this.createCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            this.searchSql = "select * from ZGW_MO_TYPE where WORK_ORDER_CODE = :WORK_ORDER_CODE ";
            this.insertSql = " insert into ZGW_MO_TYPE(WORK_ORDER_CODE,TYPE) values(:WORK_ORDER_CODE,:TYPE)";
        }
        public async Task<ZGW_MO_TYPE> Create(ZGW_MO_TYPE zGW_MO_TYPE)
        {
            var existedEntry = await createCommand.ExtractEntry<ZGW_MO_TYPE>(searchSql, zGW_MO_TYPE).ConfigureAwait(false);
            if (existedEntry != null) return existedEntry;
            var result = await createCommand.Create(insertSql, zGW_MO_TYPE).ConfigureAwait(false);

            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:ZGW_MO_TYPECreateService,Method:Create",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{LoginWindow.UserStamp.EmployeeId} {LoginWindow.UserStamp.UserName}",
                ParameterJson = JsonConvert.SerializeObject(zGW_MO_TYPE),
                SqlClauseOrFunction = $"Search Clause:{searchSql}, Insert Clause:{insertSql}"
            });

            if (result >= 1)
                return zGW_MO_TYPE;
            return null;
        }

    }
}
