using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.SGCC_Master
{
    public class ZGW_MO_T
    {
        //        PUTTIME DATE	7
        //SO_NO VARCHAR2	10
        //SO_ITEMNO VARCHAR2	8
        //SO_CODE VARCHAR2	20
        //SO_FLAG VARCHAR2	2
        //ACTUALSTARTDATE DATE	7
        //ACTUALFINISHDATE DATE	7
        //PLANTNAME VARCHAR2	40
        //WORKSHOPNAME VARCHAR2	40
        //IPOSTATUS VARCHAR2	1
        //CENTER VARCHAR2	15
        //DATASOURCE VARCHAR2	1
        //DATASOURCECREATETIME VARCHAR2	20
        //OWNERID VARCHAR2	20
        //OPENID VARCHAR2	20
        //WRITEDATE DATE	7
        //PUTSTATUS VARCHAR2	10
        //PUTMESSAGE VARCHAR2	512
        //PURCHASERHQCODE VARCHAR2	8
        //IPOTYPE VARCHAR2	1
        //SUPPLIERCODE VARCHAR2	40
        //SUPPLIERNAME VARCHAR2	50
        //IPONO VARCHAR2	12
        public String IPONO { get; set; }
        //CATEGORYCODE VARCHAR2	10
        //SUBCLASSCODE VARCHAR2	10
        //SCHEDULECODE VARCHAR2	20
        //POITEMID VARCHAR2	20
        //DATATYPE VARCHAR2	1
        //SOITEMNO VARCHAR2	6
        //MATERIALSCODE VARCHAR2	18
        public String MATERIALSCODE { get; set; }
        //MATERIALSNAME VARCHAR2	200
        public String MATERIALSNAME { get; set; }
        //MATERIALSUNIT VARCHAR2	10
        //MATERIALSDESC VARCHAR2	200
        //AMOUNT VARCHAR2	20
        public string AMOUNT { get; set; }
        //UNIT VARCHAR2	10
        //PRODUCTIDGRPNO VARCHAR2	20
        //PRODUCTIDTYPE VARCHAR2	20
        //PRODUCTMODEL VARCHAR2	30
        //PLANSTARTDATE DATE	7
        //PLANFINISHDATE DATE	7
    }
}
