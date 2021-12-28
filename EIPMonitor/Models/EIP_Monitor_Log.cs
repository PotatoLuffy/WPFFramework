using EIPMonitor.LocalInfrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model
{
    public class EIP_Monitor_Log
    {
        public DateTime OperateDateTime { get; set; }
        //SqlClauseOrFunction nvarchar2(2000),
        public String SqlClauseOrFunction { get; set; }
        //ParameterJson nvarchar2(2000),
        public String ParameterJson { get; set; }
        //OperatorUser nvarchar2(200),
        public String OperatorUser { get; set; }
        //OperateIpAddress nvarchar2(500),
        public String OperateIpAddress { get => LocalConstant.GetLocalIPAddress(); }
        //FunctionCalledInLogical nvarchar2(500)
        public String FunctionCalledInLogical { get; set; }
        public String CurrentOracleConnection { get => JsonConvert.SerializeObject(LocalConstant.OracleCurrentConnectionStringBuilder); }
        public String CurrentMESConnection { get => JsonConvert.SerializeObject(LocalConstant.CurrentMESDBConnection); }
        public String PCCredential { get => System.Security.Principal.WindowsIdentity.GetCurrent().Name; }
        public String PCName { get => Dns.GetHostName(); }
        public String ComputedBit { get; set; }

        public override string ToString()
        {
            return $"{OperateDateTime.ToString("yyyy-MM-dd HH:mm:ss")}-{SqlClauseOrFunction}-{ParameterJson}-{OperatorUser}-{OperateIpAddress}-{FunctionCalledInLogical}-{CurrentMESConnection}-{CurrentOracleConnection}-{PCCredential}-{PCName}";
        }
    }
}
