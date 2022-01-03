using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.MasterData;
using EIPMonitor.Transaction.Interface.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.MasterData.MES_MO_TO_EIP_POOLServices
{
    public class MES_MO_TO_EIP_POOLCheckService
    {
        private IUpdateCommand updateCommand;
        private String searchSql;
        private String updateSql;

        public MES_MO_TO_EIP_POOLCheckService()
        {
            updateCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            updateSql = "update MES_MO_TO_EIP_POOL set  OA_UPLOADER = :OA_UPLOADER " +
                " ,OA_UPLOADED_DATE = :OA_UPLOADED_DATE " +
                " ,UPLOAD_TO_OA_FLAG = :UPLOAD_TO_OA_FLAG " +
                " where PRODUCTION_ORDER_ID = :PRODUCTION_ORDER_ID  and  PRODUCTION_ORDER_ID in ( select IPONO from ZGW_MO_T where IPONO = :PRODUCTION_ORDER_ID and ACTUALFINISHDATE is null )";
            searchSql = "select * from MES_MO_TO_EIP_POOL where PRODUCTION_ORDER_ID = :PRODUCTION_ORDER_ID";
        }

        public async Task<List<MES_MO_TO_EIP_POOL>> Check(List<MES_MO_TO_EIP_POOL> mES_MO_TO_EIP_POOLs, IUserStamp userStamp)
        {
            for (int inx = 0; inx < mES_MO_TO_EIP_POOLs.Count; inx++)
            {
                var existsInDb = await updateCommand.ExtractEntry<MES_MO_TO_EIP_POOL>(searchSql, mES_MO_TO_EIP_POOLs[inx]).ConfigureAwait(false);
                //if (existsInDb == null)
                //{
                //    mES_MO_TO_EIP_POOLs[inx] = null;
                //}
                mES_MO_TO_EIP_POOLs[inx].OA_UPLOADER = $"{userStamp.EmployeeId} {userStamp.UserName}";
                mES_MO_TO_EIP_POOLs[inx].OA_UPLOADED_DATE = DateTime.Now;
                mES_MO_TO_EIP_POOLs[inx].UPLOAD_TO_OA_FLAG = 1;
                var result = await updateCommand.Update<MES_MO_TO_EIP_POOL>(updateSql, mES_MO_TO_EIP_POOLs[inx]).ConfigureAwait(false);
                //if (result <= 0)
                //{
                //    mES_MO_TO_EIP_POOLs[inx] = null;
                //}
            }
            mES_MO_TO_EIP_POOLs.RemoveAll(i => i == null);
            var jsonResult = JsonConvert.SerializeObject(mES_MO_TO_EIP_POOLs);

            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:MES_MO_TO_EIP_POOLCheckService,Method:Check",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{userStamp.EmployeeId} {userStamp.UserName}",
                ParameterJson = jsonResult.Length > 2000 ? JsonConvert.SerializeObject(mES_MO_TO_EIP_POOLs.Select(s => s.PRODUCTION_ORDER_ID)) : jsonResult,
                SqlClauseOrFunction = $"Search Clause:{searchSql},Insert Clause:{updateSql}",

            });
            return mES_MO_TO_EIP_POOLs;
        }
    }
}
