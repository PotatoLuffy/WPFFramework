using Dapper;
using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.SecurityModel;
using EIPMonitor.Transaction.Interface.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.SecurityServices.FunctionServices
{
    public class EIP_MONITOR_PAGESearchService
    {
        private readonly IExtractCommand extractCommand;
        private readonly string searchSql;
        public EIP_MONITOR_PAGESearchService()
        {
            extractCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            searchSql = "select * from EIP_MONITOR_PAGE where STATUS = :Status";
        }

        public Task<List<EIP_MONITOR_PAGE>> GetFunctions(EIP_MONITOR_PAGE eIP_MONITOR_PAGE)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("STATUS", eIP_MONITOR_PAGE.STATUS.GetHashCode());
            var result = extractCommand.Search<EIP_MONITOR_PAGE>(dynamicParameters, searchSql);
            return result;
        }

        public Task<List<EIP_MONITOR_PAGE>> GetFunctions(UserStamp userStamp)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("EMPLOYEEID", userStamp.EmployeeId);
            var localSearchSql = " select distinct c.*  "
                                + " from EIP_MONITOR_ROLE_USER a  "
                                + " inner join EIP_MONITOR_ROLE_PAGE b on a.ROLE_NAME = b.ROLE_NAME and a.DEPARTMENT = b.DEPARTMENT "
                                + " inner join EIP_MONITOR_PAGE c on c.PAGE_NAME = b.PAGE_NAME "
                                + " where a.EMPLOYEEID = :EMPLOYEEID ";
            var result = extractCommand.Search<EIP_MONITOR_PAGE>(dynamicParameters, localSearchSql);
            return result;
        }
    }
}
