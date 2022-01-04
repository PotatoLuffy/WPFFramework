using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Transaction.Interface.Generic
{
    public interface ICustomTransactionHelper
    {
        IDbTransaction BeginTransaction(IDbConnection conn, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit(IDbTransaction tran);
        void DisposeConn(IDbConnection conn);
        IDbConnection GetConn(OracleConnectionStringBuilder dbConnection);
        void OpenConn(IDbConnection conn);
        void Rollback(IDbTransaction tran);
    }
}
