using EIPMonitor.Databse.Generic;
using EIPMonitor.Databse.Utility;
using EIPMonitor.Domain.CustomException;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.MasterData;
using EIPMonitor.Model.Widget;
using EIPMonitor.Transaction.Interface.Generic;
using Infrastructure.Standard.Tool;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.MasterData.MES_MO_TO_EIP_POOLServices
{
    public class MES_MO_TO_EIP_POOLCheckService
    {
        private String searchSql;
        private String updateSql;
        private IUpdateCommandTransaction UpdateCommandTransaction;
        private ICustomTransactionHelper customTransactionHelper;
        private string submitToOA = "http://clou-oaadd2.szclou.com:8210/system/portal.nsf/agGetGuowangApprove.xsp";
        //private string submitToOA = "http://clou-test.szclou.com:9399/system/portal.nsf/agGetGuowangApprove.xsp";
        public MES_MO_TO_EIP_POOLCheckService()
        {
            UpdateCommandTransaction = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            customTransactionHelper = new CustomTransactionHelper();
            updateSql = "update MES_MO_TO_EIP_POOL set  OA_UPLOADER = :OA_UPLOADER " +
                " ,OA_UPLOADED_DATE = :OA_UPLOADED_DATE " +
                " ,UPLOAD_TO_OA_FLAG = :UPLOAD_TO_OA_FLAG " +
                " where PRODUCTION_ORDER_ID = :PRODUCTION_ORDER_ID  and  PRODUCTION_ORDER_ID in ( select IPONO from ZGW_MO_T where IPONO = :PRODUCTION_ORDER_ID and ACTUALFINISHDATE is null )";
            searchSql = "select * from MES_MO_TO_EIP_POOL where PRODUCTION_ORDER_ID = :PRODUCTION_ORDER_ID";
        }

        public async Task<List<MES_MO_TO_EIP_POOL>> Check(List<MES_MO_TO_EIP_POOL> mES_MO_TO_EIP_POOLs, IUserStamp userStamp)
        {
            var conn = customTransactionHelper.GetConn(LocalConstant.OracleCurrentConnectionStringBuilder);
            using (conn)
            {
                List<MES_MO_TO_EIP_POOL> requestedData = new List<MES_MO_TO_EIP_POOL>();
                var currentDate = DateTime.Now;
                var originalData = mES_MO_TO_EIP_POOLs.DeepClone();
                customTransactionHelper.OpenConn(conn);
                var tran = customTransactionHelper.BeginTransaction(conn, IsolationLevel.ReadCommitted);
                try
                {
                    for (int inx = 0; inx < mES_MO_TO_EIP_POOLs.Count; inx++)
                    {

                        var existsInDb = await UpdateCommandTransaction.ExtractEntry<MES_MO_TO_EIP_POOL>(searchSql, mES_MO_TO_EIP_POOLs[inx], conn, tran).ConfigureAwait(false);
                        //if (existsInDb == null)
                        //{
                        //    mES_MO_TO_EIP_POOLs[inx] = null;
                        //}
                        mES_MO_TO_EIP_POOLs[inx].OA_UPLOADER = $"{userStamp.EmployeeId} {userStamp.UserName}";
                        mES_MO_TO_EIP_POOLs[inx].OA_UPLOADED_DATE = currentDate;
                        mES_MO_TO_EIP_POOLs[inx].UPLOAD_TO_OA_FLAG = 1;
                        var result = await UpdateCommandTransaction.UpdateTransaction<MES_MO_TO_EIP_POOL>(updateSql, mES_MO_TO_EIP_POOLs[inx], conn, tran).ConfigureAwait(false);
                        if (result > 0) requestedData.Add(mES_MO_TO_EIP_POOLs[inx]);
                        //if (result <= 0)
                        //{
                        //    mES_MO_TO_EIP_POOLs[inx] = null;
                        //}
                    }
                    var a = new
                    {
                        OA_UPLOADED_DATE = currentDate.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        MD5_CODE = UserService.UserEncryptionService.MD5Encrypt($"clou{userStamp.EmployeeId}", currentDate.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                        OA_UPLOADER = userStamp.EmployeeId,
                        rows = requestedData
                    };
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(a), Encoding.UTF8, "application/json");

                    var responseResult = await HttpRequestCustomize.PostAsync(submitToOA, stringContent).ConfigureAwait(true);
                    var content = await responseResult.Content.ReadAsStringAsync().ConfigureAwait(false);
                    OAResponse oAResponse = JsonConvert.DeserializeObject<OAResponse>(content);
                    if (oAResponse == null)
                    {
                        throw new OAResponseFailureException("提交OA失败，请稍后5分钟再试，如果连续三次失败，请联系流程与IT管理部丁龙飞 00074729");
                    }
                    if (oAResponse.SUCCESS)
                    {
                        customTransactionHelper.Commit(tran);
                        return requestedData;
                    }


                    throw new OAResponseFailureException("提交OA失败，请稍后5分钟再试，如果连续三次失败，请联系流程与IT管理部丁龙飞 00074729");
                }
                catch (OAResponseFailureException)
                {
                    customTransactionHelper.Rollback(tran);
                    throw;
                }
                catch (Exception e)
                {
                    customTransactionHelper.Rollback(tran);
                    throw;
                }
            }
        }
    }
}
