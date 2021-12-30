using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.MasterData
{
    public class MES_MO_TO_EIP_POOL : IEquatable<MES_MO_TO_EIP_POOL>
    {
        [DisplayName("更新标记")]
        public String ExistsFlagName { get => this.ExistsFlag ? "不可更新" : "可更新"; }
        [DisplayName("提交状态")]
        public String IS_UPLOADABLE_TO_EIPName { get => this.UPLOAD_TO_OA_FLAG == 1 ? "已提交" : "未提交"; }
        [DisplayName("工单号")]
        public String PRODUCTION_ORDER_ID { get; set; }
        //public string ZTYPE { get; set; }
        //SCORE NUMBER			22
        [DisplayName("总得分")]
        public Decimal SCORE { get; set; }
        //QUALITY_EVALUATION NVARCHAR2			40
        [DisplayName("质量评估")]
        public String QUALITY_EVALUATION { get; set; }
        //        PRODUCTION_ORDER_ID VARCHAR2			20

        //MATERIAL_SCORE NUMBER			22
        [DisplayName("原材料检验得分")]
        public Decimal MATERIAL_SCORE { get; set; }
        public Decimal MATERIAL_SCORE_WEIGHT { get; set; }
        //PRODUCE_PROCESS_SCORE NUMBER			22
        [DisplayName("生产过程检验得分")]
        public Decimal PRODUCE_PROCESS_SCORE { get; set; }
        public Decimal PRODUCE_PROCESS_SCORE_WEIGHT { get; set; }
        //EXPERIMENT_SCORE NUMBER			22
        [DisplayName("试验过程检验得分")]
        public Decimal EXPERIMENT_SCORE { get; set; }
        public Decimal EXPERIMENT_SCORE_WEIGHT { get; set; }
        [DisplayName("物料名称")]
        public string MATERIALSNAME { get; set; }
        //CREATOR NVARCHAR2			100
        //[DisplayName("创建人")]
        public String CREATOR { get; set; }
        //CREATED_DATE DATE			7
        //[DisplayName("创建时间")]
        public DateTime CREATED_DATE { get; set; }
        //CONFIRMER NVARCHAR2			100
        //[DisplayName("确认人")]
        public String CONFIRMER { get; set; }
        //CONFIRMED_DATE DATE			7
        //[DisplayName("确认时间")]
        public DateTime CONFIRMED_DATE { get; set; }
        public String OA_UPLOADER { get; set; }
        //CONFIRMED_DATE DATE			7
        //[DisplayName("确认时间")]
        public DateTime OA_UPLOADED_DATE { get; set; }
        //IS_UPLOADABLE_TO_EIP NUMBER			22
        //[DisplayName("审核状态")]
        public Decimal IS_UPLOADABLE_TO_EIP { get; set; }
        public Decimal UPLOAD_TO_OA_FLAG { get; set; }

        //HAS_UPLOADED_FLAG NUMBER			22
        //[DisplayName("是否已经上传")]
        public Decimal HAS_UPLOADED_FLAG { get; set; }

        public String HAS_UPLOADED_FLAGName { get => (this.HAS_UPLOADED_FLAG == 1).ToString(); }

        public Boolean ExistsFlag { get; set; }
        public static MES_MO_TO_EIP_POOL GetDefaultInstance(IUserStamp userStamp)
        {
            return new MES_MO_TO_EIP_POOL() { HAS_UPLOADED_FLAG = 0, IS_UPLOADABLE_TO_EIP = 0, UPLOAD_TO_OA_FLAG = 0, CREATED_DATE = DateTime.Now, CREATOR = $"{userStamp.EmployeeId} {userStamp.UserName}" };
        }
        public void CalculateQuality_Evaluation()
        {
            if (this.SCORE > 80 && this.SCORE <= 100) { this.QUALITY_EVALUATION = "优质品"; return; }
            if (this.SCORE > 40 && this.SCORE <= 80) { this.QUALITY_EVALUATION = "良好品"; return; }
            this.QUALITY_EVALUATION = "合格品";
        }
        public bool Equals(MES_MO_TO_EIP_POOL other)
        {
            if (other == null) return false;
            if (this.PRODUCTION_ORDER_ID.Equals(other.PRODUCTION_ORDER_ID)) return true;
            return false;
        }
        public override bool Equals(object obj)
        {
            MES_MO_TO_EIP_POOL other = obj as MES_MO_TO_EIP_POOL;
            return this.Equals(other);
        }
        public override int GetHashCode()
        {
            return this.PRODUCTION_ORDER_ID.GetHashCode();
        }
    }
}
