using EIPMonitor.Model;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EIPMonitor.LocalInfrastructure
{
    public static class LocalConstant
    {
        public static ILog Logger;
        public static Boolean IsAdmin = false;
        
        public static void SetUpConnection(string connectionString)
        {
            //log4net.Config.XmlConfigurator.Configure(new FileInfo("Log4Net.config"));
            Hierarchy hier = LogManager.GetRepository() as Hierarchy;
            if (hier != null)
            {
                var adoNetAppenders = hier.GetAppenders().OfType<AdoNetAppender>();
                foreach (var adoNetAppender in adoNetAppenders)
                {
                    adoNetAppender.ConnectionString = connectionString;
                    adoNetAppender.ActivateOptions();
                }

            }
            log4net.Util.LogLog.InternalDebugging = true;
        }

        public static void SetUpLogger()
        {
            Logger = LogManager.GetLogger("EIP Monitor");
            //Logger.Info(LocalConstant.OracleCurrentConnectionStringBuilder.ToString(), new Exception("Info Level"));
            //Logger.Debug(LocalConstant.OracleCurrentConnectionStringBuilder.ToString(), new Exception("Debug Level"));
            //Logger.Error("Exception ", new Exception("Error Level"));
            //Logger.Warn("Warn", new Exception("Warn Level"));
        }
        //private static readonly ILog Logger = LogManager.GetLogger();

        public static OracleConnectionStringBuilder oracleConnectionStringBuilder = new OracleConnectionStringBuilder()
        {
            Password = "SGCCTRANS",
            UserID = "SGCCTRANS",
            DataSource = $"{"10.98.99.53"}:{"1521"}/{"sgccprd"}",
            Pooling = true
        };
        public static OracleConnectionStringBuilder oracleConnectionStringBuilderTest = new OracleConnectionStringBuilder()
        {
            Password = "GW!clou2020",
            UserID = "sgcc",
            DataSource = $"{"10.98.93.210"}:{"1521"}/{"sgccqas"}",
            Pooling = true
        };
        public static OracleConnectionStringBuilder OracleCurrentConnectionStringBuilder = new OracleConnectionStringBuilder()
        {
            Password = "SGCCTRANS",
            UserID = "SGCCTRANS",
            DataSource = $"{"10.98.99.53"}:{"1521"}/{"sgccprd"}",
            Pooling = true
        };
        public static SqlConnectionStringBuilder MESTestDB = new SqlConnectionStringBuilder()
        {
            DataSource = "10.98.99.8",
            InitialCatalog = "OrBitXI",
            MultipleActiveResultSets = true,
            UserID = "sa",
            Password = "MES(123)",
            ConnectTimeout = 3_600,
        };
        public static SqlConnectionStringBuilder MESStandardDB = new SqlConnectionStringBuilder()
        {
            DataSource = "10.98.99.7",
            InitialCatalog = "OrBitXI",
            MultipleActiveResultSets = true,
            UserID = "sa",
            Password = "MES(123)",
            ConnectTimeout = 3_600,
        };
        public static SqlConnectionStringBuilder CurrentMESDBConnection = new SqlConnectionStringBuilder()
        {
            DataSource = "10.98.99.8",
            InitialCatalog = "OrBitXI",
            MultipleActiveResultSets = true,
            UserID = "sa",
            Password = "MES(123)",
            
            ConnectTimeout = 3_600,
        };
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            StringBuilder sb = new StringBuilder(1000);
            foreach (var ip in host.AddressList)
                sb.Append($"{ip.AddressFamily.ToString()}:{ip.ToString()}");

            return sb.ToString();
        }


    }
}
