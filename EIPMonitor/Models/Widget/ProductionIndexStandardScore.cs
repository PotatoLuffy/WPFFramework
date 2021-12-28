using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.Widget
{
    public static class ProductionIndexStandardScore
    {
        private static Dictionary<ProductionTestItem, Dictionary<ProductionDataItem, Int32>> StandardScoreDic = new Dictionary<ProductionTestItem, Dictionary<ProductionDataItem, int>>()
        {
            { ProductionTestItem.Battery,new Dictionary<ProductionDataItem, int>(){{ ProductionDataItem.StandardVoltage,10 }} },
            { ProductionTestItem.CrystalResonator,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.StandardFrequentError, 20 } } },
            { ProductionTestItem.CurrentTransformer,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.CONTRAST_VALUE,20 } } },
            { ProductionTestItem.Experimentation_DailyTimingEror,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.InspectionConclusion,10 } } },
            { ProductionTestItem.Experimentation_DailyTimingDetailEror,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.RealError,20 } } },
            { ProductionTestItem.Experimentation_HighVoltageInsulationTestion,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.ElectricLeakageMeasuredValue,20 } } },
            { ProductionTestItem.Experimentation_IntrinsicError,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.InspectionConclusion,10 } } },
            { ProductionTestItem.Experimentation_IntrinsicErrorDetail,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.AverageError,40 } } },
            { ProductionTestItem.LiquidCrystal,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.InspectionConclusion,20 } } },
            { ProductionTestItem.PhotoElectricCoupler,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.InspectionConclusion,15 } } },
            { ProductionTestItem.ProductionProcessInspection_BatteryCurrent,new Dictionary<ProductionDataItem, int>(){
                { ProductionDataItem.InspectionConclusion, 20},
                { ProductionDataItem.CurrentMeasuredValue,30}
            } },
            { ProductionTestItem.ProductionProcessInspection_PCBAMountingInspection,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.InspectionConclusion,20 } } },
            { ProductionTestItem.ProductionProcessInspection_SingleBoardTesting,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.InspectionConclusion,30 } } },
            { ProductionTestItem.TransientDiode,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.InspectionConclusion,15 } } },
            { ProductionTestItem.VoltageDependentResistor,new Dictionary<ProductionDataItem, int>(){ { ProductionDataItem.ErrorValue,20 } } },
        };
        public static Int32 GetStandardScore(ProductionTestItem productionTestItem, ProductionDataItem productionDataItem)
        {
            if (!StandardScoreDic.ContainsKey(productionTestItem)) return 0;
            var item = StandardScoreDic[productionTestItem];
            if (!item.ContainsKey(productionDataItem)) return 0;
            return item[productionDataItem];
        }
    }
}
