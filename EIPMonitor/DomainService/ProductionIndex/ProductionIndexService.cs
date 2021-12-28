using EIPMonitor.DomainServices.SGCC;
using EIPMonitor.Model.ProductionIndex;
using EIPMonitor.Model.SGCC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.ProductionIndex
{
    public class ProductionIndexService
    {
        private static readonly EM_EXP_PRO_BASICERSearchServices eM_EXP_PRO_BASICERSearchServices = new EM_EXP_PRO_BASICERSearchServices();
        private static readonly EM_EXP_PRO_DAYTIMEERSearchService eM_EXP_PRO_DAYTIMEERSearchService = new EM_EXP_PRO_DAYTIMEERSearchService();
        private static readonly EM_EXP_PRO_VOLTESTSearchService eM_EXP_PRO_VOLTESTSearchService = new EM_EXP_PRO_VOLTESTSearchService();
        private static readonly PD_MQC_BTRSerchService pD_MQC_BTRSerchService = new PD_MQC_BTRSerchService();
        private static readonly PD_MQC_CRSearchService pD_MQC_CRSearchService = new PD_MQC_CRSearchService();
        private static readonly PD_MQC_CTSearchService pD_MQC_CTSearchService = new PD_MQC_CTSearchService();
        private static readonly PD_MQC_LCSearchService pD_MQC_LCSearchService = new PD_MQC_LCSearchService();
        private static readonly PD_MQC_OCSearchService pD_MQC_OCSearchService = new PD_MQC_OCSearchService();
        private static readonly PD_MQC_TVSSearchService pD_MQC_TVSSearchService = new PD_MQC_TVSSearchService();
        private static readonly PD_MQC_VDRSearchService pD_MQC_VDRSearchService = new PD_MQC_VDRSearchService();
        private static readonly PP_TEST_BATCURSearchService pP_TEST_BATCURSearchService = new PP_TEST_BATCURSearchService();
        private static readonly PP_TEST_FCTSearchService pP_TEST_FCTSearchService = new PP_TEST_FCTSearchService();
        private static readonly PP_TEST_PCBAOISearchService pP_TEST_PCBAOISearchService = new PP_TEST_PCBAOISearchService();
        private static readonly EM_EXP_PRO_BASICER_DETAIL_DETAILService eM_EXP_PRO_BASICER_DETAIL_DETAILService = new EM_EXP_PRO_BASICER_DETAIL_DETAILService();
        private readonly ConcurrentBag<IPIScoreDetail> pIScoreDetails = new ConcurrentBag<IPIScoreDetail>();
        #region Parameter
        private readonly EM_EXP_PRO_BASICER eM_EXP_PRO_BASICER;
        private readonly EM_EXP_PRO_DAYTIMEER eM_EXP_PRO_DAYTIMEER;
        private readonly PD_MQC_BTR pD_MQC_BTR;
        private readonly PD_MQC_CR pD_MQC_CR;
        private readonly PD_MQC_CT pD_MQC_CT;
        private readonly PD_MQC_LC pD_MQC_LC;
        private readonly PD_MQC_OC pD_MQC_OC;
        private readonly PD_MQC_TVS pD_MQC_TVS;
        private readonly PD_MQC_VDR pD_MQC_VDR;
        private readonly PP_TEST_BATCUR pP_TEST_BATCUR;
        private readonly PP_TEST_FCT pP_TEST_FCT;
        private readonly PP_TEST_PCBAOI pP_TEST_PCBAOI;
        private readonly EM_EXP_PRO_VOLTEST eM_EXP_PRO_VOLTEST;
        private readonly EM_EXP_PRO_BASICER_DETAIL eM_EXP_PRO_BASICER_DETAIL;
        #endregion
        #region raw data
        private  List<EM_EXP_PRO_BASICER> eM_EXP_PRO_BASICERs;
        private  List<EM_EXP_PRO_DAYTIMEER> eM_EXP_PRO_DAYTIMEERs;
        private  List<PD_MQC_BTR> pD_MQC_BTRs;
        private  List<PD_MQC_CR> pD_MQC_CRs;
        private  List<PD_MQC_CT> pD_MQC_CTs;
        private  List<PD_MQC_LC> pD_MQC_LCs;
        private  List<PD_MQC_OC> pD_MQC_OCs;
        private  List<PD_MQC_TVS> pD_MQC_TVSs;
        private  List<PD_MQC_VDR> pD_MQC_VDRs;
        private  List<PP_TEST_BATCUR> pP_TEST_BATCURs;
        private  List<PP_TEST_FCT> pP_TEST_FCTs;
        private  List<PP_TEST_PCBAOI> pP_TEST_PCBAOIs;
        private List<EM_EXP_PRO_VOLTEST> eM_EXP_PRO_VOLTESTs;
        private List<EM_EXP_PRO_BASICER_DETAIL> eM_EXP_PRO_BASICER_DETAILs;
        #endregion
        public ProductionIndexService(DateTime? beginDate, DateTime? endDate, String beginOrder, String endOrder)
        {
            #region parameter
            eM_EXP_PRO_BASICER_DETAIL = new EM_EXP_PRO_BASICER_DETAIL()
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            eM_EXP_PRO_BASICER = new EM_EXP_PRO_BASICER()
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            eM_EXP_PRO_DAYTIMEER = new EM_EXP_PRO_DAYTIMEER() 
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            pD_MQC_BTR = new PD_MQC_BTR() 
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            pD_MQC_CR = new PD_MQC_CR() 
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            pD_MQC_CT = new PD_MQC_CT() 
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            pD_MQC_LC = new PD_MQC_LC() 
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            pD_MQC_OC = new PD_MQC_OC() 
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            pD_MQC_TVS = new PD_MQC_TVS()
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            pD_MQC_VDR = new PD_MQC_VDR()
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            pP_TEST_BATCUR = new PP_TEST_BATCUR()
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            pP_TEST_FCT = new PP_TEST_FCT() 
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            pP_TEST_PCBAOI = new PP_TEST_PCBAOI() 
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            eM_EXP_PRO_VOLTEST = new EM_EXP_PRO_VOLTEST()
            {
                BeginOrder = String.IsNullOrWhiteSpace(beginOrder) ? endOrder : beginOrder,
                EndOrder = String.IsNullOrWhiteSpace(endOrder) ? beginOrder : endOrder,
                BeginDate = beginDate ?? endDate,
                EndDate = endDate ?? beginDate,
                STANDARDVERSION = 2
            };
            #endregion

        }
        public List<IPIScoreDetail> GetProductionIndexData()
        {
            GetAllTheTestData();
            return pIScoreDetails.ToList();
        }
        public String GetTheTestData()
        {
            GetAllTheTestData();
            return $"eM_EXP_PRO_BASICERs:{eM_EXP_PRO_BASICERs?.Count ?? 0}{Environment.NewLine}" +
                $"eM_EXP_PRO_DAYTIMEERs:{eM_EXP_PRO_DAYTIMEERs?.Count ?? 0}{Environment.NewLine}" +
                $"pD_MQC_BTRs:{pD_MQC_BTRs?.Count ?? 0}{Environment.NewLine}" +
                $"pD_MQC_CRs:{pD_MQC_CRs?.Count ?? 0}{Environment.NewLine}" +
                $"pD_MQC_CTs:{pD_MQC_CTs?.Count ?? 0}{Environment.NewLine}" +
                $"pD_MQC_LCs:{pD_MQC_LCs?.Count ?? 0}{Environment.NewLine}" +
                $"pD_MQC_OCs:{pD_MQC_OCs?.Count ?? 0}{Environment.NewLine}" +
                $"pD_MQC_TVSs:{pD_MQC_TVSs?.Count ?? 0}{Environment.NewLine}" +
                $"pD_MQC_VDRs:{pD_MQC_VDRs?.Count ?? 0}{Environment.NewLine}" +
                $"pP_TEST_BATCURs:{pP_TEST_BATCURs?.Count ?? 0}{Environment.NewLine}" +
                $"pP_TEST_FCTs:{pP_TEST_FCTs?.Count ?? 0}{Environment.NewLine}" +
                $"pP_TEST_PCBAOIs:{pP_TEST_PCBAOIs?.Count ?? 0}{Environment.NewLine}" +
                $"eM_EXP_PRO_VOLTESTs:{eM_EXP_PRO_VOLTESTs?.Count ?? 0}";
        }
        private void GetAllTheTestData()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(GetBASICERData());
            tasks.Add(GeteM_EXP_PRO_DAYTIMEERData());
            tasks.Add(GetpD_MQC_BTRData());
            tasks.Add(GetpD_MQC_CRData());
            tasks.Add(GetpD_MQC_CTData());
            //tasks.Add(GetpD_MQC_LCData());
            tasks.Add(GetEM_EXP_PRO_BASICER_DETAILData());
            tasks.Add(GetpD_MQC_OCData());
            tasks.Add(GetpD_MQC_TVSData());
            tasks.Add(GetpD_MQC_VDRData());
            tasks.Add(GetpP_TEST_BATCURData());
            tasks.Add(GetpP_TEST_FCTData());
            tasks.Add(GetpP_TEST_PCBAOIData());
            tasks.Add(GeteM_EXP_PRO_VOLTESTData());
            tasks.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount).ForAll(f => f.Wait());
            Task.WaitAll(tasks.ToArray());
        }
        private async Task GetBASICERData()
        {
            eM_EXP_PRO_BASICERs = await eM_EXP_PRO_BASICERSearchServices.GetEntries(eM_EXP_PRO_BASICER).ConfigureAwait(false);
            var calculatedResult = eM_EXP_PRO_BASICERs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new IntrinsicErrorIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());

                return result;
            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new IntrinsicErrorIndex(eM_EXP_PRO_BASICER.BeginOrder, new List<EM_EXP_PRO_BASICER>(), new List<EM_EXP_PRO_BASICER>()));
                return;
            }
            foreach(var el  in calculatedResult)
              pIScoreDetails.Add(el);
        }
        private async Task GetpD_MQC_BTRData()
        {
            pD_MQC_BTRs = await pD_MQC_BTRSerchService.GetEntries(pD_MQC_BTR).ConfigureAwait(false);
            var calculatedResult = pD_MQC_BTRs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new BatteryIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
               //pIScoreDetails.Add(result);
                return result;
            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new BatteryIndex(pD_MQC_BTR.BeginOrder, new List<PD_MQC_BTR>(), new List<PD_MQC_BTR>()));
                return;
            }

            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
        }
        private async Task GeteM_EXP_PRO_DAYTIMEERData()
        {
            eM_EXP_PRO_DAYTIMEERs = await eM_EXP_PRO_DAYTIMEERSearchService.GetEntries(eM_EXP_PRO_DAYTIMEER).ConfigureAwait(false);
            var calculatedResult = eM_EXP_PRO_DAYTIMEERs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new DailyTimingErrorIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(result);
                return result;

            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new DailyTimingErrorIndex(eM_EXP_PRO_DAYTIMEER.BeginOrder, new List<EM_EXP_PRO_DAYTIMEER>(), new List<EM_EXP_PRO_DAYTIMEER>()));
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
            var calculatedResult1 = eM_EXP_PRO_DAYTIMEERs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new DailyTimingDetailErrorIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(result);
                return result;
            });

            if (calculatedResult1.Count() <= 0)
            {
                pIScoreDetails.Add(new DailyTimingDetailErrorIndex(eM_EXP_PRO_DAYTIMEER.BeginOrder, new List<EM_EXP_PRO_DAYTIMEER>(), new List<EM_EXP_PRO_DAYTIMEER>()));
            }
            foreach (var el in calculatedResult1)
                pIScoreDetails.Add(el);
        }
        private async Task GetpD_MQC_CRData()
        {
            pD_MQC_CRs = await pD_MQC_CRSearchService.GetEntries(pD_MQC_CR).ConfigureAwait(false);
            var calculatedResult = pD_MQC_CRs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new CrystalResonatorIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(result);
                return result;

            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new CrystalResonatorIndex(pD_MQC_CR.BeginOrder, new List<PD_MQC_CR>(), new List<PD_MQC_CR>()));
                return;
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
        }
        private async Task GetpD_MQC_CTData()
        {
            pD_MQC_CTs = await pD_MQC_CTSearchService.GetEntries(pD_MQC_CT).ConfigureAwait(false);
            var calculatedResult = pD_MQC_CTs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new CurrentTransformerIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
               // pIScoreDetails.Add(result);
                return result;

            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new CurrentTransformerIndex(pD_MQC_CT.BeginOrder, new List<PD_MQC_CT>(), new List<PD_MQC_CT>()));
                return;
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
        }
        private async Task GetpD_MQC_LCData()
        {
            pD_MQC_LCs = await pD_MQC_LCSearchService.GetEntries(pD_MQC_LC).ConfigureAwait(false);
            var calculatedResult = pD_MQC_LCs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new LiquidCrystalIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(result);
                return result;

            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new CurrentTransformerIndex(pD_MQC_CT.BeginOrder, new List<PD_MQC_CT>(), new List<PD_MQC_CT>()));
                return;
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
        }
        private async Task GetpD_MQC_OCData()
        {
            pD_MQC_OCs = await pD_MQC_OCSearchService.GetEntries(pD_MQC_OC).ConfigureAwait(false);
            var calculatedResult = pD_MQC_OCs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new PhotoElectricCouplerIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(result);
                return result;

            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new PhotoElectricCouplerIndex(pD_MQC_OC.BeginOrder, new List<PD_MQC_OC>(), new List<PD_MQC_OC>()));
                return;
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
        }
        private async Task GetpD_MQC_TVSData()
        {
            pD_MQC_TVSs = await pD_MQC_TVSSearchService.GetEntries(pD_MQC_TVS).ConfigureAwait(false);
            var calculatedResult = pD_MQC_TVSs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new TransientDiodeIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(result);
                return result;

            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new TransientDiodeIndex(pD_MQC_TVS.BeginOrder, new List<PD_MQC_TVS>(), new List<PD_MQC_TVS>()));
                return;
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
        }
        private async Task GetpD_MQC_VDRData()
        {
            pD_MQC_VDRs = await pD_MQC_VDRSearchService.GetEntries(pD_MQC_VDR).ConfigureAwait(false);
            var calculatedResult = pD_MQC_VDRs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new VoltageDependentResistorIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(result);
                return result;

            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new VoltageDependentResistorIndex(pD_MQC_VDR.BeginOrder, new List<PD_MQC_VDR>(), new List<PD_MQC_VDR>()));
                return;
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
        }
        private async Task GetpP_TEST_BATCURData()
        {
            pP_TEST_BATCURs = await pP_TEST_BATCURSearchService.GetEntries(pP_TEST_BATCUR).ConfigureAwait(false);
            var calculatedResult = pP_TEST_BATCURs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s => {
                var inspectionResult = new BatteryCurrentInspectionIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
               // var currentValueResult = new BatteryCurrentMeasuredIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(inspectionResult);
                //pIScoreDetails.Add(currentValueResult);
                return inspectionResult;
            });
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);

            var calculatedResult1 = pP_TEST_BATCURs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s => {
                //var inspectionResult = new BatteryCurrentInspectionIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                var currentValueResult = new BatteryCurrentMeasuredIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(inspectionResult);
                //pIScoreDetails.Add(currentValueResult);
                return currentValueResult;
            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new BatteryCurrentInspectionIndex(pP_TEST_BATCUR.BeginOrder, new List<PP_TEST_BATCUR>(), new List<PP_TEST_BATCUR>()));
            }
            if (calculatedResult1.Count() <= 0)
            {
                pIScoreDetails.Add(new BatteryCurrentMeasuredIndex(pP_TEST_BATCUR.BeginOrder, new List<PP_TEST_BATCUR>(), new List<PP_TEST_BATCUR>()));
            }
            foreach (var el in calculatedResult1)
                pIScoreDetails.Add(el);
        }
        private async Task GetpP_TEST_FCTData()
        {
            pP_TEST_FCTs = await pP_TEST_FCTSearchService.GetEntries(pP_TEST_FCT).ConfigureAwait(false);
            var calculatedResult = pP_TEST_FCTs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new SingleBoardTestingIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(result);
                return result;

            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new SingleBoardTestingIndex(pP_TEST_FCT.BeginOrder, new List<PP_TEST_FCT>(), new List<PP_TEST_FCT>()));
                return;
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
        }
        private async Task GetpP_TEST_PCBAOIData()
        {
            pP_TEST_PCBAOIs = await pP_TEST_PCBAOISearchService.GetEntries(pP_TEST_PCBAOI).ConfigureAwait(false);
            var calculatedResult = pP_TEST_PCBAOIs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s =>
            {
                var result = new PCBMountingInspectionIndex(s.Key, s.ToList(), s.Where(w => w.CONCLUSION == 0).ToList());
                //pIScoreDetails.Add(result);
                return result;
            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new PCBMountingInspectionIndex(pP_TEST_PCBAOI.BeginOrder, new List<PP_TEST_PCBAOI>(), new List<PP_TEST_PCBAOI>()));
                return;
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
        }
        private async Task GeteM_EXP_PRO_VOLTESTData()
        {
            eM_EXP_PRO_VOLTESTs = await eM_EXP_PRO_VOLTESTSearchService.GetEntries( eM_EXP_PRO_VOLTEST).ConfigureAwait(false);
            var calculatedResult = eM_EXP_PRO_VOLTESTs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s => {
                var totalList = s.ToList();
                var passList = s.Where(w => w.CONCLUSION == 0).ToList();
                var result = new HighVoltageInsulationTestIndex(s.Key, totalList, passList);
                //pIScoreDetails.Add(result);
                return result;
            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new HighVoltageInsulationTestIndex(eM_EXP_PRO_VOLTEST.BeginOrder, new List<EM_EXP_PRO_VOLTEST>(), new List<EM_EXP_PRO_VOLTEST>()));
                return;
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
            //pIScoreDetails
        }
        public async Task GetEM_EXP_PRO_BASICER_DETAILData()
        {
            eM_EXP_PRO_BASICER_DETAILs = await eM_EXP_PRO_BASICER_DETAIL_DETAILService.GetEntries(eM_EXP_PRO_BASICER_DETAIL).ConfigureAwait(false);
            var calculatedResult = eM_EXP_PRO_BASICER_DETAILs.GroupBy(g => g.PRODUCTION_ORDER_ID).Select(s => {
                var totalList = s.ToList();
                var passList = s.Where(w => w.CONCLUSION == 0).ToList();
                var result = new IntrinsicErrorDetailIndex(s.Key, totalList, passList);
                //pIScoreDetails.Add(result);
                return result;
            });
            if (calculatedResult.Count() <= 0)
            {
                pIScoreDetails.Add(new IntrinsicErrorDetailIndex(eM_EXP_PRO_VOLTEST.BeginOrder, new List<EM_EXP_PRO_BASICER_DETAIL>(), new List<EM_EXP_PRO_BASICER_DETAIL>()));
                return;
            }
            foreach (var el in calculatedResult)
                pIScoreDetails.Add(el);
        }
    }
}
