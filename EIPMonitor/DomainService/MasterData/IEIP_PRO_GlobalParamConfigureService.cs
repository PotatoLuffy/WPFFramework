using EIPMonitor.Model.MasterData;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.MasterData
{
    public interface IEIP_PRO_GlobalParamConfigureService
    {
        Task<EIP_PRO_GlobalParamConfigure> ExtractConfiguration(EIP_PRO_GlobalParameter eIP_PRO_GlobalParameter);
    }
}