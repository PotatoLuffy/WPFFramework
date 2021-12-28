using EIPMonitor.Model;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.UserService
{
    public interface IEIPProductionIndexUsersLogin
    {
        Task<EIPProductionIndexUsers> Login(EIPProductionIndexUsers eIPProductionIndexUsers);
    }
}