using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.Widget
{
    public enum ProductionDataCategory
    {
        Material=0,
        Producing=1,
        QualityTesting = 2
    }
    public static class ProductionDataCategoryMapper
    {
        
        private static readonly IReadOnlyDictionary<ProductionDataCategory, String> ValueNameMapper = new Dictionary<ProductionDataCategory, string>()
        {
            { ProductionDataCategory.Material,"原材料检验"},
            { ProductionDataCategory.Producing,"生产过程"},
            { ProductionDataCategory.QualityTesting,"试验过程"}
        };
        public static string Translator(this ProductionDataCategory productionTestItem)
        {
            if (!ValueNameMapper.ContainsKey(productionTestItem)) return "不存在";
            return ValueNameMapper[productionTestItem];
        }
    }
}
