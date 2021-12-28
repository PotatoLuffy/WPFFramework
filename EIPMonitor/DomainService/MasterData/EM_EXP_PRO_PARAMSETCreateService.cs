using EIPMonitor.Databse.Generic;
using EIPMonitor.Domain.CustomException;
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

namespace EIPMonitor.DomainServices.MasterData
{
    public class EM_EXP_PRO_PARAMSETCreateService
    {
        private ICreateCommand createCommand;
        private string insertSql;
        private string VerifyAssertNoUniqueSql;
        private string VerifyPCBCodeUniqueSql;
        public EM_EXP_PRO_PARAMSETCreateService() 
        {
            this.createCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            this.VerifyAssertNoUniqueSql = " select * from EM_EXP_PRO_PARAMSET where ASSET_MANAGEMENT_NO = :ASSET_MANAGEMENT_NO";
            this.VerifyPCBCodeUniqueSql = " select * from EM_EXP_PRO_PARAMSET where PCB_CODE = :PCB_CODE";
            this.insertSql = " insert into EM_EXP_PRO_PARAMSET "
                         + " (STANDARDVERSION  "
         + " ,PRODUCTION_ORDER_ID  "
         + " ,WORK_ORDER_CODE "
         + " ,TEST_CODE  "
         + " ,PLANT_CODE  "
         + " ,CHECK_TIME  "
         + " ,CREATE_TIME  "
         + " ,MODEL_CODE  "
         + " ,INFO_TYPE_CODE   "
         + " ,SOFTWARE_VERSION  "
         + " ,NAMEPLATE_CODE  "
         + " ,CONCLUSION "
         + " ,TEST_NAME  "
         + " ,PCB_CODE  "
         + " ,DEVICE_NO  "
         + " ,TABLE_NO "
         + " ,TABLE_ADDRESS "
         + " ,ASSET_MANAGEMENT_NO) "
+ " values "
         + " (:STANDARDVERSION  "
         + " ,:PRODUCTION_ORDER_ID  "
         + " ,:WORK_ORDER_CODE "
         + " ,:TEST_CODE  "
         + " ,:PLANT_CODE  "
         + " ,:CHECK_TIME  "
         + " ,:CREATE_TIME  "
         + " ,:MODEL_CODE  "
         + " ,:INFO_TYPE_CODE   "
         + " ,:SOFTWARE_VERSION  "
         + " ,:NAMEPLATE_CODE  "
         + " ,:CONCLUSION "
         + " ,:TEST_NAME  "
         + " ,:PCB_CODE  "
         + " ,:DEVICE_NO  "
         + " ,:TABLE_NO "
         + " ,:TABLE_ADDRESS "
         + " ,:ASSET_MANAGEMENT_NO ) ";
        }
        public async Task<EM_EXP_PRO_PARAMSET> Create(EM_EXP_PRO_PARAMSET eM_EXP_PRO_PARAMSET)
        {
            var verifyAsset = await createCommand.ExtractEntry<EM_EXP_PRO_PARAMSET>(VerifyAssertNoUniqueSql, eM_EXP_PRO_PARAMSET).ConfigureAwait(false);
            var verifyPCBCode = await createCommand.ExtractEntry<EM_EXP_PRO_PARAMSET>(VerifyPCBCodeUniqueSql, eM_EXP_PRO_PARAMSET).ConfigureAwait(false);
            if (verifyAsset != null || verifyPCBCode != null)
                throw new DuplicatedEntryException("PCB编号或者资产编号已存在。");
            var result = await createCommand.Create<EM_EXP_PRO_PARAMSET>(this.insertSql, eM_EXP_PRO_PARAMSET).ConfigureAwait(false);
            EIP_Monitor_LogCreateService.Log(new EIP_Monitor_Log()
            {
                FunctionCalledInLogical = "DomainService:EM_EXP_PRO_PARAMSETCreateService,Method:Create",
                OperateDateTime = DateTime.Now,
                OperatorUser = $"{IocKernel.Get<UserStamp>().EmployeeId} {IocKernel.Get<UserStamp>().UserName}",
                ParameterJson = JsonConvert.SerializeObject(eM_EXP_PRO_PARAMSET),
                SqlClauseOrFunction = $"verifyAsset Clause:{VerifyAssertNoUniqueSql},VerifyPCBCodeUniqueSql Clause:{VerifyPCBCodeUniqueSql},insert Clause:{insertSql} "
            }) ;
            if (result <= 0) return null;
            else return eM_EXP_PRO_PARAMSET;
        }
    }
}
