using EIPMonitor.Model;
using EIPMonitor.Model.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.MasterData
{
    public static class MES_MO_TO_EIP_POOLGenerator
    {

        public static List<MES_MO_TO_EIP_POOL> Aggregate(this List<ZCL_SIMUL_D> zCL_SIMUL_Ds,IUserStamp userStamp)
        {
            var result = zCL_SIMUL_Ds.GroupBy(g => $"{g.WORK_ORDER_CODE}").Select(s =>
            {

                var ins = MES_MO_TO_EIP_POOL.GetDefaultInstance(userStamp);
                ins.PRODUCTION_ORDER_ID = s.Max(m => m.WORK_ORDER_CODE);
                ins.PRODUCE_PROCESS_SCORE = s.Where(w => w.ZTYPE == "生产过程检验").Sum(s1 => s1.SCORE);
                ins.MATERIAL_SCORE = s.Where(w => w.ZTYPE == "原材料检验").Sum(s1 => s1.SCORE);
                ins.EXPERIMENT_SCORE = s.Where(w => w.ZTYPE == "试验过程检验").Sum(s1 => s1.SCORE);
                ins.PRODUCE_PROCESS_SCORE_WEIGHT = s.Where(w => w.ZTYPE == "生产过程检验").DefaultIfEmpty().Max(s1 => s1?.ZWEIGHT ?? 0);
                ins.EXPERIMENT_SCORE_WEIGHT = s.Where(w => w.ZTYPE == "试验过程检验").DefaultIfEmpty().Max(s1 => s1?.ZWEIGHT ?? 0);
                ins.MATERIAL_SCORE_WEIGHT = s.Where(w => w.ZTYPE == "原材料检验").DefaultIfEmpty().Max(s1 => s1?.ZWEIGHT ?? 0);
                ins.SCORE = ins.MATERIAL_SCORE * ins.MATERIAL_SCORE_WEIGHT + ins.EXPERIMENT_SCORE * ins.EXPERIMENT_SCORE_WEIGHT + ins.PRODUCE_PROCESS_SCORE * ins.PRODUCE_PROCESS_SCORE_WEIGHT;
                ins.MATERIALSNAME = s.Max(m => m.MATERIALSNAME);
                ins.CalculateQuality_Evaluation();
                return ins;
            });
            return result.ToList();
        }
    }
}
