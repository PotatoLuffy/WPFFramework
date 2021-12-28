using EIPMonitor.Databse.Generic;
using EIPMonitor.DomainServices.UserService;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Transaction.Interface.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices
{
    public static class EIP_Monitor_LogCreateService
    {
        //CRUDService:
        private static ICreateCommand createCommand = new CRUDService(LocalConstant.oracleConnectionStringBuilder);
        private static string insertSql = " insert into EIP_Monitor_Log "
                                        + " ( "
                                        + " SqlClauseOrFunction, "
                                        + " ParameterJson, "
                                        + " OperatorUser, "
                                        + " OperateIpAddress, "
                                        + " FunctionCalledInLogical, "
                                        + " CurrentOracleConnection, "
                                        + " CurrentMESConnection, "
                                        + " ComputedBit, "
                                        + " PCCredential, "
                                        + " PCName "
                                        + " ) "
                                        + " values "
                                        + " ( "
                                        + " :SqlClauseOrFunction, "
                                        + " :ParameterJson, "
                                        + " :OperatorUser, "
                                        + " :OperateIpAddress, "
                                        + " :FunctionCalledInLogical, "
                                        + " :CurrentOracleConnection, "
                                        + " :CurrentMESConnection, "
                                        + " :ComputedBit, "
                                        + " :PCCredential, "
                                        + " :PCName "
                                        + " ) ";
        public static void Log(EIP_Monitor_Log eIP_Monitor_Log)
        {
            eIP_Monitor_Log.ComputedBit = UserEncryptionService.Encrypt("EIP_Monitor_Log", eIP_Monitor_Log.ToString());
            createCommand.Create<EIP_Monitor_Log>(insertSql, eIP_Monitor_Log).Wait();
        }
    }
}
