using Dapper;
using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model.MasterData;
using EIPMonitor.Transaction.Interface.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.MasterData
{
    public class ZCL_TYPESearchService
    {
        //CRUDService:ICreateCommand,IUpdateCommand, IExtractCommand
        private readonly IExtractCommand extractCommand;
        public string searchSql;
        public ZCL_TYPESearchService()
        {
            extractCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            searchSql = "select * from zcl_type where ZVALID = :ZVALID and SUBCLASSCODE = :SUBCLASSCODE";
        }

        public Task<List<ZCL_TYPE>> GetZCL_Type(ZCL_TYPE zCL_TYPE)
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("ZVALID", zCL_TYPE.ZVALID);
            dynamicParameter.Add("SUBCLASSCODE", zCL_TYPE.SUBCLASSCODE);
            var result = extractCommand.Search<ZCL_TYPE>(dynamicParameter, searchSql);
            return result;
        }

    }
}
