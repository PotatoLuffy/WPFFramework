using Infrastructure.Standard.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.Widget
{
    public static class ProductionIndexPolicy
    {
        //public static Dictionary<ProductionDataItem, Func<Decimal, Int32>> policyMapper = new Dictionary<ProductionDataItem, Func<decimal, decimal>>() 
        //{
        //    { ProductionDataItem.StandardVoltage, CalculateStandardVoltage },
        //    { ProductionDataItem.ElectricLeakageMeasuredValue, CalculateElectricLeakageMeasuredValue},
        //    { ProductionDataItem.InspectionConclusion,CalculateInspectionConclusion},
        //    { ProductionDataItem.TransformRateMeasuredValue,CalculateTransformRateMeasuredValue},
        //    { ProductionDataItem.CurrentMeasuredValue,CalculateCurrentMeasuredValue},
        //    {ProductionDataItem.InspectionConclusion1, CalculateInspectionConclusion1},
        //    {ProductionDataItem.InspectionConclusion2, CalculateInspectionConclusion2},
        //    {ProductionDataItem.AverageError, CalculateAverageError},
        //};
        public static Dictionary<ProductionDataCategory, Dictionary<ProductionTestItem, Dictionary<ProductionDataItem, Func<Decimal, decimal>>>> policyMapper = new Dictionary<ProductionDataCategory, Dictionary<ProductionTestItem, Dictionary<ProductionDataItem, Func<decimal, decimal>>>>()
        {
            #region Material
            { ProductionDataCategory.Material,new Dictionary<ProductionTestItem, Dictionary<ProductionDataItem, Func<decimal, decimal>>>(){
                { ProductionTestItem.Battery, new Dictionary<ProductionDataItem, Func<decimal, decimal>>() {
                    { ProductionDataItem.StandardVoltage,(testValue)=> {              
                                                                          if (testValue >= 3.6m) return 10;
                                                                          if (testValue < 3.6m) return 6;
                                                                          return 4;
                                                                      } 
                    } 
                                                                                                        } 
                },
                { ProductionTestItem.CrystalResonator, new Dictionary<ProductionDataItem, Func<decimal, decimal>>() {
                    { ProductionDataItem.StandardFrequentError,(testValue)=> {
                                                                          if (testValue >0 && testValue <= 10m) return 20;
                                                                          if (testValue >10m && testValue <= 20m) return 16;
                                                                          if (testValue >20m) return 12;
                                                                          return 8;
                                                                      }
                    }
                                                                                                        }
                },
                { ProductionTestItem.CurrentTransformer, new Dictionary<ProductionDataItem, Func<decimal, decimal>>() {
                    { ProductionDataItem.CONTRAST_VALUE,(testValue)=> {
                                                                          if (testValue >0 && testValue <= 0.1m) return 20;
                                                                          if (testValue >0.1m && testValue <= 0.2m) return 16;
                                                                          if (testValue >0.2m) return 12;
                                                                          return 8;
                                                                      }
                    }
                                                                                                        }
                },
                { ProductionTestItem.LiquidCrystal, new Dictionary<ProductionDataItem, Func<decimal, decimal>>() {
                    { ProductionDataItem.InspectionConclusion,(testValue)=> {
                                                                            if (testValue >= 0.99m) return 20;
                                                                            if (testValue >= 0.95m && testValue < 0.99m) return 16;
                                                                            if (testValue < 0.95m) return 12;
                                                                            return 8;
                                                                      }
                    }
                                                                                                        }
                },
                { ProductionTestItem.PhotoElectricCoupler, new Dictionary<ProductionDataItem, Func<decimal, decimal>>() {
                    { ProductionDataItem.InspectionConclusion,(testValue)=> {
                                                                            if (testValue >= 0.99m) return 15;
                                                                            if (testValue >= 0.95m && testValue < 0.99m) return 13;
                                                                            if (testValue < 0.95m) return 9;
                                                                            return 6;
                                                                      }
                    }
                                                                                                        }
                },
                { ProductionTestItem.TransientDiode, new Dictionary<ProductionDataItem, Func<decimal, decimal>>() {
                    { ProductionDataItem.InspectionConclusion,(testValue)=> {
                                                                            if (testValue >= 0.99m) return 15;
                                                                            if (testValue >= 0.95m && testValue < 0.99m) return 13;
                                                                            if (testValue < 0.95m) return 9;
                                                                            return 6;
                                                                      }
                    }
                                                                                                        }
                },
                { ProductionTestItem.VoltageDependentResistor, new Dictionary<ProductionDataItem, Func<decimal, decimal>>() {
                    { ProductionDataItem.ErrorValue,(testValue)=> {
                                                                            if (testValue < 0.05m) return 20;
                                                                            if (testValue >= 0.05m && testValue <= 0.1m) return 16;
                                                                            if (testValue > 0.1m) return 12;
                                                                            return 8;
                                                                      }
                    }
                                                                                                        }
                },
            } },
            #endregion
            #region Production Processing
            { 
                ProductionDataCategory.Producing,new Dictionary<ProductionTestItem, Dictionary<ProductionDataItem, Func<decimal, decimal>>>()
                {
                    { ProductionTestItem.ProductionProcessInspection_BatteryCurrent,new Dictionary<ProductionDataItem, Func<decimal, decimal>>()
                                                                                                                                            {
                        {ProductionDataItem.InspectionConclusion,(testValue)=>{
                                                                                 if(testValue>=0.99m) return 20;
                                                                                 if(testValue >= 0.95m && testValue < 0.99m) return 16;
                                                                                 if(testValue >0 && testValue < 0.95m) return 12;
                                                                                 return 8;
                                                                              } 
                        },
                        {ProductionDataItem.CurrentMeasuredValue,(testValue)=>{
                                                                                 if(testValue >0 && testValue<=10) return 30;
                                                                                 if(testValue > 10 && testValue <= 15) return 26;
                                                                                 if(testValue > 15 && testValue <= 20) return 22;
                                                                                 if(testValue > 20 && testValue <= 25) return 20;
                                                                                 if(testValue > 25) return 18;
                                                                                 return 0;
                                                                              }
                        }, 
                                                                                                                                           } 
                    },
                    { ProductionTestItem.ProductionProcessInspection_SingleBoardTesting,new Dictionary<ProductionDataItem, Func<decimal, decimal>>()
                                                                                                                                            {
                        {ProductionDataItem.InspectionConclusion,(testValue)=>{
                                                                                 if(testValue>=0.99m) return 30;
                                                                                 if(testValue >= 0.97m && testValue < 0.99m) return 28;
                                                                                 if(testValue >= 0.95m && testValue < 0.97m) return 22;
                                                                                 if(testValue >0 && testValue < 0.95m) return 18;
                                                                                 return 12;
                                                                              }
                        }
                                                                                                                                           }
                    },
                    { ProductionTestItem.ProductionProcessInspection_PCBAMountingInspection,new Dictionary<ProductionDataItem, Func<decimal, decimal>>()
                                                                                                                                            {
                        {ProductionDataItem.InspectionConclusion,(testValue)=>{
                                                                                 if(testValue>=0.99m) return 20;
                                                                                 if(testValue >= 0.95m && testValue < 0.99m) return 18;
                                                                                 if(testValue >= 0.90m && testValue < 0.95m) return 16;
                                                                                 if(testValue >0 && testValue < 0.90m) return 12;
                                                                                 return 8;
                                                                              }
                        }
                                                                                                                                           }
                    },
                } 
            },
            #endregion
            #region Experiment
            { 
                ProductionDataCategory.QualityTesting, new Dictionary<ProductionTestItem, Dictionary<ProductionDataItem, Func<decimal, decimal>>>() 
                                                        {
                                                           {
                                                              ProductionTestItem.Experimentation_HighVoltageInsulationTestion, new Dictionary<ProductionDataItem, Func<decimal, decimal>>()
                                                              {
                                                                  { 
                                                                      ProductionDataItem.ElectricLeakageMeasuredValue,(testValue)=>{
                                                                                                                                     if(testValue >0 && testValue <= 1) return 20;
                                                                                                                                     if(testValue>1 && testValue < 5) return 16;
                                                                                                                                     if(testValue >=5 ) return 12;
                                                                                                                                     return 8;
                                                                                                                                   } 
                                                                  }
                                                              }
                                                           },
                                                           {
                                                              ProductionTestItem.Experimentation_IntrinsicError, new Dictionary<ProductionDataItem, Func<decimal, decimal>>()
                                                              {
                                                                  {
                                                                      ProductionDataItem.InspectionConclusion,(testValue)=>{
                                                                                                                                     if(testValue >= 0.99m) return 10;
                                                                                                                                     if(testValue>0.99m && testValue <= 0.97m) return 9.2m;
                                                                                                                                     if(testValue>0.97m && testValue <= 0.95m) return 8.4m;
                                                                                                                                     if(testValue > 0 && testValue < 0.95m ) return 6;
                                                                                                                                     return 4;
                                                                                                                                   }
                                                                  }
                                                              }
                                                           },
                                                           {
                                                              ProductionTestItem.Experimentation_IntrinsicErrorDetail, new Dictionary<ProductionDataItem, Func<decimal, decimal>>()
                                                              {
                                                                  {
                                                                      ProductionDataItem.AverageError,(testValue)=>{
                                                                                                                                     if( testValue > 0 && testValue <= 0.2m) return 20;
                                                                                                                                     if(testValue>0.2m && testValue <= 0.3m) return 18;
                                                                                                                                     if(testValue>0.3m && testValue <= 0.5m) return 16;
                                                                                                                                     if(testValue > 0.5m ) return 12;
                                                                                                                                     return 8;
                                                                                                                                   }
                                                                  }
                                                              }
                                                           },
                                                           {
                                                              ProductionTestItem.Experimentation_DailyTimingEror, new Dictionary<ProductionDataItem, Func<decimal, decimal>>()
                                                              {
                                                                  {
                                                                      ProductionDataItem.InspectionConclusion,(testValue)=>{
                                                                                                                                     if(testValue >= 0.99m) return 10;
                                                                                                                                     if(testValue>0.99m && testValue <= 0.97m) return 9.2m;
                                                                                                                                     if(testValue>0.97m && testValue <= 0.95m) return 8.4m;
                                                                                                                                     if(testValue > 0 && testValue < 0.95m ) return 6;
                                                                                                                                     return 6;
                                                                                                                                   }
                                                                  }
                                                              }
                                                           },
                                                           {
                                                              ProductionTestItem.Experimentation_DailyTimingDetailEror, new Dictionary<ProductionDataItem, Func<decimal, decimal>>()
                                                              {
                                                                  {
                                                                      ProductionDataItem.RealError,(testValue)=>{
                                                                                                                                     if( testValue > 0 && testValue <= 0.15m) return 40;
                                                                                                                                     if(testValue>0.15m && testValue <= 0.3m) return 36;
                                                                                                                                     if(testValue>0.3m && testValue <= 0.6m) return 30;
                                                                                                                                     if(testValue > 0.6m ) return 24;
                                                                                                                                     return 16;
                                                                                                                                   }
                                                                  }
                                                              }
                                                           },
                                                        }
            }
            #endregion
        };
        static ProductionIndexPolicy()
        {
        }
        private static Int32 CalculateAverageError(Decimal testValue)
        {
            if (testValue > 0 && testValue <= 0.15m) return 40;
            if (testValue > 0.15m && testValue <= 0.3m) return 36;
            if (testValue > 0.3m && testValue <= 0.6m) return 30;
            if (testValue > 0.6m) return 24;
            return 16;
        }
        private static Int32 CalculateStandardVoltage(Decimal testValue)
        {
            if (testValue >= 3.6m) return 10;
            if (testValue < 3.6m) return 6;
            return 4;
        }
        private static Int32 CalculateElectricLeakageMeasuredValue(Decimal testValue)
        {
            if (testValue <= 1 && testValue > 0) return 20;
            if (testValue > 1 && testValue < 5) return 16;
            if (testValue >= 5) return 12;
            return 8;
        }
        private static Int32 CalculateInspectionConclusion(Decimal testValue)
        {
            if (testValue >= 0.99m) return 15;
            if (testValue >= 0.95m && testValue < 0.99m) return 13;
            if (testValue < 0.95m) return 9;
            return 6;
        }
        private static Int32 CalculateInspectionConclusion1(Decimal testValue)
        {
            if (testValue >= 0.99m) return 20;
            if (testValue >= 0.9m && testValue < 0.99m) return 10;
            return 0;
        }
        private static Int32 CalculateInspectionConclusion2(Decimal testValue)
        {
            if (testValue >= 0.99m) return 15;
            if (testValue >= 0.9m && testValue < 0.99m) return 10;
            return 0;
        }
        private static Int32 CalculateTransformRateMeasuredValue(Decimal testValue)
        {
            if (testValue >= 400) return 15;
            if (testValue >= 300 && testValue < 400) return 8;
            return 0;
        }
        private static Int32 CalculateCurrentMeasuredValue(Decimal testValue)
        {
            if (testValue <= 10 && testValue > 0) return 25;
            if (testValue > 10 && testValue <= 15) return 20;
            if (testValue > 15 && testValue <= 20) return 15;
            if (testValue > 20 && testValue <= 25) return 10;
            if (testValue > 25 && testValue <= 30) return 5;
            return 0;
        }
    }
}
