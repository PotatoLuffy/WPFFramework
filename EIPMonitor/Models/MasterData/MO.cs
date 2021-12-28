using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EIPMonitor.Model.MasterData
{
    public sealed class MO : IEquatable<MO>
    {
        //MOId char no	12	     	     	no no  no Chinese_PRC_CI_AS
        [Key]
        [StringLength(12)]
        public String MOId { get; set; }
        //MOName nvarchar    no	100	     	     	yes(n/a)   (n/a)	Chinese_PRC_CI_AS
        [StringLength(100)]
        public String MOName { get; set; }
        //MODescription   nvarchar no	200	     	     	yes(n/a)   (n/a)	Chinese_PRC_CI_AS
        [StringLength(200)]
        public String MODescription { get; set; }
        //MOTypeId    char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String MOTypeId { get; set; }
        //PlannedDateFrom datetime    no	8	     	     	yes(n/a)   (n/a)	NULL
        public DateTime? PlannedDateFrom { get; set; }
        //PlannedDateTo   datetime no	8	     	     	yes(n/a)   (n/a)	NULL
        public DateTime? PlannedDateTo { get; set; }
        //ExecuteDateFrom datetime no	8	     	     	yes(n/a)   (n/a)	NULL
        public DateTime? ExecuteDateFrom { get; set; }
        //ExecuteDateTo   datetime no	8	     	     	yes(n/a)   (n/a)	NULL
        public DateTime? ExecuteDateTo { get; set; }
        //ProductId   char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String ProductId { get; set; }
        //MOQtyRequired int no	4	10   	0    	yes(n/a)   (n/a)	NULL
        public Int32? MOQtyRequired { get; set; }
        //MOQtyDone   int no	4	10   	0    	yes(n/a)   (n/a)	NULL
        public Int32? MOQtyDone { get; set; }
        //PriorityLevel   char no	5	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(5)]
        public String PriorityLevel { get; set; }
        //CreateUserId char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String CreateUserId { get; set; }
        //CreateResourceId char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String CreateResourceId { get; set; }
        //SOId char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String SOId { get; set; }
        //CustomerId char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String CustomerId { get; set; }
        //MOStatus char no	2	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(2)]
        public String MOStatus { get; set; }
        //ParentMOId char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String ParentMOId { get; set; }
        //FactoryId char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String FactoryId { get; set; }
        //WorkflowId char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String WorkflowId { get; set; }
        //ERPDate datetime    no	8	     	     	yes(n/a)   (n/a)	NULL
        public DateTime? ERPDate { get; set; }
        //ApproveUserId   char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String ApproveUserId { get; set; }
        //ApproveDate datetime    no	8	     	     	yes(n/a)   (n/a)	NULL
        public DateTime? ApproveDate { get; set; }
        //CreateDate  datetime no	8	     	     	yes(n/a)   (n/a)	NULL
        public DateTime? CreateDate { get; set; }
        //Factory nvarchar no	40	     	     	yes(n/a)   (n/a)	Chinese_PRC_CI_AS
        [StringLength(40)]
        public String Factory { get; set; }
        //WareHouse   nvarchar no	40	     	     	yes(n/a)   (n/a)	Chinese_PRC_CI_AS
        [StringLength(40)]
        public String WareHouse { get; set; }
        //MOSection   char no	10	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(10)]
        public String MOSection { get; set; }
        //MORelationBill nvarchar    no	400	     	     	yes(n/a)   (n/a)	Chinese_PRC_CI_AS
        [StringLength(400)]
        public String MORelationBill { get; set; }
        //IsMerged    bit no	1	     	     	yes(n/a)   (n/a)	NULL
        public Boolean? IsMerged { get; set; }
        //IsExceedControl bit no	1	     	     	yes(n/a)   (n/a)	NULL
        public Boolean? IsExceedControl { get; set; }
        //MOQtyStart  int no	4	10   	0    	yes(n/a)   (n/a)	NULL
        public int? MOQtyStart { get; set; }
        //MOQtyReleased   int no	4	10   	0    	yes(n/a)   (n/a)	NULL
        public int? MOQtyReleased { get; set; }
        //BOMVersion  nvarchar no	400	     	     	yes(n/a)   (n/a)	Chinese_PRC_CI_AS
        [StringLength(400)]
        public String BOMVersion { get; set; }
        //IsNoControl char no	1	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(1)]
        public String IsNoControl { get; set; }
        //DoubleSideQty int no	4	10   	0    	yes(n/a)   (n/a)	NULL
        public Int32? DoubleSideQty { get; set; }
        //OutSourcingType varchar no	50	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(50)]
        public String OutSourcingType { get; set; }
        //IsOutSourcing int no	4	10   	0    	yes(n/a)   (n/a)	NULL
        public Int32? IsOutSourcing { get; set; }
        //OutSourcingQty  int no	4	10   	0    	yes(n/a)   (n/a)	NULL
        public Int32? OutSourcingQty { get; set; }
        //OutSourcingUserId   char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String OutSourcingUserId { get; set; }
        //OutSourcingDate datetime    no	8	     	     	yes(n/a)   (n/a)	NULL
        public DateTime? OutSourcingDate { get; set; }
        //IsFreePassInAfterLoadingTest    bit no	1	     	     	yes(n/a)   (n/a)	NULL
        public Boolean? IsFreePassInAfterLoadingTest { get; set; }
        //IsSeries    bit no	1	     	     	yes(n/a)   (n/a)	NULL
        public Boolean? IsSeries { get; set; }
        //IsSMTNoControlMaterial  bit no	1	     	     	yes(n/a)   (n/a)	NULL
        public Boolean? IsSMTNoControlMaterial { get; set; }
        //InspectionQty   int no	4	10   	0    	yes(n/a)   (n/a)	NULL
        public Int32? InspectionQty { get; set; }
        //PCBSideBComponentQty    int no	4	10   	0    	yes(n/a)   (n/a)	NULL
        public Int32? PCBSideBComponentQty { get; set; }
        //PCBComponentCost    float no	8	53   	NULL yes(n/a)   (n/a)	NULL
        public float? PCBComponentCost { get; set; }
        //ProductType varchar no	4	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(4)]
        public String ProductType { get; set; }
        //FactoryCode char no	4	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(4)]
        public String FactoryCode { get; set; }
        //MRPControlerCode char no	3	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(3)]
        public String MRPControlerCode { get; set; }
        //IsScattered bit no	1	     	     	yes(n/a)   (n/a)	NULL
        public Boolean? IsScattered { get; set; }
        //IsHuaweiMO  bit no	1	     	     	yes(n/a)   (n/a)	NULL
        public Boolean? IsHuaweiMO { get; set; }
        //UOMId   char no	12	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(12)]
        public String UOMId { get; set; }
        //WBSNumber varchar no	10	     	     	yes no  yes Chinese_PRC_CI_AS
        [StringLength(10)]
        public String WBSNumber { get; set; }

        public bool Equals(MO other)
        {
            if (other.MOId == this.MOId || other.MOName == this.MOName) return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            MO other = obj as MO;
            if (other == null) return false;
            return this.Equals(other);
        }
        public override int GetHashCode()
        {
            return this.MOName.GetHashCode();
        }
    }
}
