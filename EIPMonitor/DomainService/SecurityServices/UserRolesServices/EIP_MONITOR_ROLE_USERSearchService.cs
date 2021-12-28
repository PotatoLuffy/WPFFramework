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

namespace EIPMonitor.DomainServices.SecurityServices.UserRolesServices
{
    public class EIP_MONITOR_ROLE_USERSearchService
    {
        private readonly IExtractCommand extractCommand;
        private readonly string searchSql;
        public EIP_MONITOR_ROLE_USERSearchService()
        {
            extractCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            searchSql = "select * from EIP_MONITOR_ROLE_USER where EMPLOYEEID = :EMPLOYEEID ";
        }

        public Task<List<EIP_MONITOR_ROLE_USER>> GetSpecificUserRoles(EIP_MONITOR_ROLE_USER eIP_MONITOR_PAGE)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("EMPLOYEEID", eIP_MONITOR_PAGE.EMPLOYEEID);
            var result = extractCommand.Search<EIP_MONITOR_ROLE_USER>(dynamicParameters, searchSql);
            return result;
        }
    }
}
