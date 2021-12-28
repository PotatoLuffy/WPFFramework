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
    public class MES_MO_TO_EIP_POOLCreateService
    {
        private ICreateCommand createCommand;
        private IUpdateCommand updateCommand;
        private String searchSql;
        private String createSql;
        private String updateSql;
        public MES_MO_TO_EIP_POOLCreateService()
        {
            createCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            updateCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            updateSql = "update MES_MO_TO_EIP_POOL set  SCORE = :SCORE " +
                " ,QUALITY_EVALUATION = :QUALITY_EVALUATION " +
                " ,MATERIAL_SCORE = :MATERIAL_SCORE " +
                " ,PRODUCE_PROCESS_SCORE = :PRODUCE_PROCESS_SCORE " +
                " ,EXPERIMENT_SCORE = :EXPERIMENT_SCORE " +
                " ,MATERIALSNAME = :MATERIALSNAME " +
                " where PRODUCTION_ORDER_ID = :PRODUCTION_ORDER_ID ";
            createSql = " insert into MES_MO_TO_EIP_POOL " +
                        " ( " +
                        " PRODUCTION_ORDER_ID, " +
                        " MATERIAL_SCORE, " +
                        " PRODUCE_PROCESS_SCORE, " +
                        " EXPERIMENT_SCORE, " +
                        " SCORE, " +
                        " QUALITY_EVALUATION, " +
                        " CREATOR, " +
                        " CREATED_DATE, " +
                        " IS_UPLOADABLE_TO_EIP, " +
                        " HAS_UPLOADED_FLAG, " +
                        " MATERIALSNAME," +
                        " UPLOAD_TO_OA_FLAG " +
                        " ) " +
                        " values " +
                        " ( " +
                            " :PRODUCTION_ORDER_ID, " +
                            " :MATERIAL_SCORE, " +
                            " :PRODUCE_PROCESS_SCORE, " +
                            " :EXPERIMENT_SCORE, " +
                            " :SCORE, " +
                            " :QUALITY_EVALUATION, " +
                            " :CREATOR, " +
                            " :CREATED_DATE, " +
                            " :IS_UPLOADABLE_TO_EIP, " +
                            " :HAS_UPLOADED_FLAG,  " +
                            " :MATERIALSNAME," +
                            " :UPLOAD_TO_OA_FLAG " +
                        " ) ";
            searchSql = "select * from MES_MO_TO_EIP_POOL where PRODUCTION_ORDER_ID = :PRODUCTION_ORDER_ID";
        }

        public async Task<List<MES_MO_TO_EIP_POOL>> Create(List<MES_MO_TO_EIP_POOL> mES_MO_TO_EIP_POOLs, UserStamp userStamp)
        {
            for (int inx=0;inx< mES_MO_TO_EIP_POOLs.Count;inx++)
            {
                var existsInDb = await createCommand.ExtractEntry<MES_MO_TO_EIP_POOL>(searchSql, mES_MO_TO_EIP_POOLs[inx]).ConfigureAwait(false);
                if (existsInDb != null && existsInDb.IS_UPLOADABLE_TO_EIP == 1)
                {
                    mES_MO_TO_EIP_POOLs[inx] = null;
                    continue;
                }
                if (existsInDb != null)
                {
                    var updateRows = await updateCommand.Update<MES_MO_TO_EIP_POOL>(updateSql, mES_MO_TO_EIP_POOLs[inx]).ConfigureAwait(false);
                    if (updateRows <= 0)
                        mES_MO_TO_EIP_POOLs[inx] = null;
                    continue;
                }
                mES_MO_TO_EIP_POOLs[inx].IS_UPLOADABLE_TO_EIP = 0;
                var result = await createCommand.Create<MES_MO_TO_EIP_POOL>(createSql, mES_MO_TO_EIP_POOLs[inx]).ConfigureAwait(false);
                if (result <= 0)
                {
                    mES_MO_TO_EIP_POOLs[inx] = null;
                }
            }
            mES_MO_TO_EIP_POOLs.RemoveAll(i => i == null);
            var jsonResult = JsonConvert.SerializeObject(mES_MO_TO_EIP_POOLs);
            
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:MES_MO_TO_EIP_POOLCreateService,Method:Create",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{userStamp.EmployeeId} {userStamp.UserName}",
                ParameterJson = jsonResult.Length>2000? JsonConvert.SerializeObject(mES_MO_TO_EIP_POOLs.Select(s=>s.PRODUCTION_ORDER_ID)): jsonResult,
                SqlClauseOrFunction = $"Search Clause:{searchSql},Insert Clause:{createSql}",
                
            }) ;
            return mES_MO_TO_EIP_POOLs;
        }
    }
}
