using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.Widget
{
    public enum ProductionDataItem
    {
        StandardVoltage=0,
        InspectionConclusion=1,
        ElectricLeakageMeasuredValue=2,
        TransformRateMeasuredValue = 3,
        CurrentMeasuredValue=4,
        InspectionConclusion1 = 5,
        InspectionConclusion2 = 6,
        AverageError=7,
        RealError = 8,
        CONTRAST_VALUE = 9,
        StandardFrequentError = 10,
        ErrorValue = 11,
    }
    public static class ProductionDataItemMapper
    {
        private static readonly IReadOnlyDictionary<ProductionDataItem, String> ValueNameMapper = new Dictionary<ProductionDataItem, String>
        {
            { ProductionDataItem.StandardVoltage,"标称电压" },
            { ProductionDataItem.InspectionConclusion,"检测结论"},
            { ProductionDataItem.ElectricLeakageMeasuredValue,"漏电流实测值"},
            { ProductionDataItem.TransformRateMeasuredValue,"传输比实测值"},
            { ProductionDataItem.CurrentMeasuredValue,"电流实测值"},
            { ProductionDataItem.InspectionConclusion1,"检测结论"},
            { ProductionDataItem.InspectionConclusion2,"检测结论"},
            { ProductionDataItem.AverageError,"平均误差"},
            { ProductionDataItem.RealError,"实际误差"},
            { ProductionDataItem.CONTRAST_VALUE,"比差值"},
            { ProductionDataItem.StandardFrequentError,"标称频差"},
            { ProductionDataItem.ErrorValue,"误差值"}
        };
        public static string Translator(this ProductionDataItem productionTestItem)
        {
            if (!ValueNameMapper.ContainsKey(productionTestItem)) return "不存在";
            return ValueNameMapper[productionTestItem];
        }
    }
}
