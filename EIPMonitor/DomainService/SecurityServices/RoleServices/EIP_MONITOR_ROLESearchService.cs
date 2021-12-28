using Dapper;
using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model.SecurityModel;
using EIPMonitor.Transaction.Interface.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.SecurityServices.RoleServices
{
    public class EIP_MONITOR_ROLESearchService
    {
        private readonly IExtractCommand extractCommand;
        private readonly string searchSql;
        public EIP_MONITOR_ROLESearchService()
        {
            extractCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            searchSql = "select * from EIP_MONITOR_ROLE ";
        }

        public Task<List<EIP_MONITOR_ROLE>> GetRoles(EIP_MONITOR_ROLE eIP_MONITOR_PAGE)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            var result = extractCommand.Search<EIP_MONITOR_ROLE>(dynamicParameters, searchSql);
            return result;
        }
    }
}
