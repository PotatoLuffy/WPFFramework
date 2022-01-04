using EIPMonitor.Transaction.Interface.Generic;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Databse.Utility
{
    public class CustomTransactionHelper : ICustomTransactionHelper
    {
        public IDbConnection GetConn(OracleConnectionStringBuilder dbConnection)
        {
            var conn = new OracleConnection(dbConnection.ToString());
            return conn;
        }
        public void OpenConn(IDbConnection conn)
        {
            conn.Open();
        }
        public void DisposeConn(IDbConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }
        public IDbTransaction BeginTransaction(IDbConnection conn, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var tran = conn.BeginTransaction(isolationLevel);
            return tran;
        }
        public void Commit(IDbTransaction tran)
        {
            tran.Commit();
        }
        public void Rollback(IDbTransaction tran)
        {
            tran.Rollback();
        }
    }
}
