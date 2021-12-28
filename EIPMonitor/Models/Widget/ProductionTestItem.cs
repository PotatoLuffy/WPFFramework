using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.Widget
{
    public enum ProductionTestItem
    {
        Battery=0,
        CurrentTransformer=1,
        CrystalResonator=2,
        TransientDiode=3,
        VoltageDependentResistor=4,
        LiquidCrystal=5,
        PhotoElectricCoupler=6,
        ProductionProcessInspection_PCBAMountingInspection=7,
        ProductionProcessInspection_SingleBoardTesting=8,//Function Current Transformer
        ProductionProcessInspection_BatteryCurrent = 9 ,
        Experimentation_HighVoltageInsulationTestion=10,
        Experimentation_IntrinsicError = 11,
        Experimentation_DailyTimingEror = 12,
        Experimentation_IntrinsicErrorDetail = 13,
        Experimentation_DailyTimingDetailEror = 14,

    }
    public static class ProductionTestItemMapper
    {
       
        private static readonly IReadOnlyDictionary<ProductionTestItem, String> ValueNameMapper = new Dictionary<ProductionTestItem, String>() 
        {
            { ProductionTestItem.Battery,"电池(BTR)"},
            { ProductionTestItem.CurrentTransformer,"电流互感器(CT)"},
            { ProductionTestItem.CrystalResonator,"晶体谐振器(CR)" },
            { ProductionTestItem.TransientDiode,"瞬变二极管(TVS)" },
            { ProductionTestItem.VoltageDependentResistor,"压敏电阻" },
            { ProductionTestItem.LiquidCrystal,"液晶"},
            { ProductionTestItem.PhotoElectricCoupler,"光电耦合器"},
            { ProductionTestItem.ProductionProcessInspection_PCBAMountingInspection,"PCB板贴装检验" },
            { ProductionTestItem.ProductionProcessInspection_SingleBoardTesting,"单板测试（FCT)" },
            { ProductionTestItem.ProductionProcessInspection_BatteryCurrent,"电池电流" },
            { ProductionTestItem.Experimentation_HighVoltageInsulationTestion,"耐压测试" },
            { ProductionTestItem.Experimentation_IntrinsicError,"基本误差" },
            { ProductionTestItem.Experimentation_DailyTimingEror,"日计时误差" },
             { ProductionTestItem.Experimentation_IntrinsicErrorDetail,"基本误差详情" },
              { ProductionTestItem.Experimentation_DailyTimingDetailEror,"日计时误差详情" },

        };
        public static string Translator(this ProductionTestItem productionTestItem)
        {
            if (!ValueNameMapper.ContainsKey(productionTestItem)) return "不存在";
            return ValueNameMapper[productionTestItem];
        }
    }
}
