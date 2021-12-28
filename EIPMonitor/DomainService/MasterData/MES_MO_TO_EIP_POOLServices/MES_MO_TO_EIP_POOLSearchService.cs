using Dapper;
using EIPMonitor.Databse.Generic;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model.MasterData;
using EIPMonitor.Transaction.Interface.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.MasterData.MES_MO_TO_EIP_POOLServices
{
    public class MES_MO_TO_EIP_POOLSearchService
    {
        private readonly IExtractCommand extractCommand;
        private string searchSql;
        
        public MES_MO_TO_EIP_POOLSearchService()
        {
            extractCommand = new CRUDService(LocalConstant.OracleCurrentConnectionStringBuilder);
            searchSql = "select a.*,case when b.ACTUALFINISHDATE is not null then 1 else 0 end  ExistsFlag from MES_MO_TO_EIP_POOL a inner join (select IPONO,max(ACTUALFINISHDATE)  ACTUALFINISHDATE from ZGW_MO_T group by IPONO) b on a.PRODUCTION_ORDER_ID = b.IPONO ";
        }

        public Task<List<MES_MO_TO_EIP_POOL>> GeMES_MO_TO_EIP_POOL(List<MES_MO_TO_EIP_POOL> mES_MO_TO_EIP_POOLs)
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            StringBuilder sb = new StringBuilder();
            for (Int32 inx = 0; inx < mES_MO_TO_EIP_POOLs.Count; inx++)
            {
                var paramName = $"param{inx}";
                dynamicParameter.Add(paramName, mES_MO_TO_EIP_POOLs[inx].PRODUCTION_ORDER_ID);
                sb.Append($":{paramName},");
            }
            var localSearchSql = $" {searchSql} where a.PRODUCTION_ORDER_ID in ({sb.ToString().TrimEnd(',')}) ";
            var result = extractCommand.Search<MES_MO_TO_EIP_POOL>(dynamicParameter, localSearchSql);
            return result;
        }

        public Task<List<MES_MO_TO_EIP_POOL>> GeMES_MO_TO_EIP_POOL(List<String> mES_MO_TO_EIP_POOLs, char firstLetter)
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            StringBuilder sb = new StringBuilder();
            for (Int32 inx = 0; inx < mES_MO_TO_EIP_POOLs.Count; inx++)
            {
                var paramName = $"param{inx}";
                dynamicParameter.Add(paramName, mES_MO_TO_EIP_POOLs[inx]);
                sb.Append($":{paramName},");
            }
            var localSearchSql = $" {searchSql} where a.PRODUCTION_ORDER_ID in ({sb.ToString().TrimEnd(',')})  and a.PRODUCTION_ORDER_ID like '{firstLetter}%'  ";
            var result = extractCommand.Search<MES_MO_TO_EIP_POOL>(dynamicParameter, localSearchSql);
            return result;
        }
        public Task<List<MES_MO_TO_EIP_POOL>> GetQueryData( string beginOrder,string endOrder , char firstLetter )
        {
            String localSearchSql;
            DynamicParameters dynamicParameter = new DynamicParameters();
            if (String.IsNullOrWhiteSpace(beginOrder) || String.IsNullOrWhiteSpace(endOrder))
            {
                localSearchSql = $" {searchSql} where a.PRODUCTION_ORDER_ID like '{firstLetter}%' ";
            }
            else
            {
                localSearchSql = $"{searchSql} where a.PRODUCTION_ORDER_ID between :beginOrder and :endOrder and a.PRODUCTION_ORDER_ID like '{firstLetter}%' ";
                dynamicParameter.Add("beginOrder", beginOrder);
                dynamicParameter.Add("endOrder", endOrder);
            }
            var result = extractCommand.Search<MES_MO_TO_EIP_POOL>(dynamicParameter, localSearchSql);
            return result;
        }
    }
}
