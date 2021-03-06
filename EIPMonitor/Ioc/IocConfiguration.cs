using EIPMonitor.DomainService;
using EIPMonitor.DomainServices.MasterData;
using EIPMonitor.DomainServices.UserService;
using EIPMonitor.IDomainService;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.ViewDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace EIPMonitor.Ioc
{

    public class IocConfiguration
    {
        public IocConfiguration()
        {

        }
        public void InitializeModules()
        {
            IocKernel.UnityContainer.RegisterSingleton<IUserStamp, UserStamp>();
            IocKernel.UnityContainer.RegisterSingleton<IEIP_PRO_GlobalParamConfigureService, EIP_PRO_GlobalParamConfigureService>();
            IocKernel.UnityContainer.RegisterSingleton<IEIPProductionIndexUsersLogin, EIPProductionIndexUsersLogin>();
            IocKernel.UnityContainer.RegisterSingleton<IRequestLimitControlService, RequestLimitControlService>();
            IocKernel.UnityContainer.RegisterType<IModelDialog, LoginViewDialog>("LoginViewDialog");
            IocKernel.UnityContainer.RegisterType<IModelDialog, MainWindowDialog>("MainWindowDialog");
        }
    }
}
