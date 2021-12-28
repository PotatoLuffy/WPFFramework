using EIPMonitor.DomainServices.UserService;
using EIPMonitor.Model;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Ioc
{
    public class IocConfiguration:NinjectModule
    {
        public override void Load()
        {
            Bind<IUserStamp>().To<UserStamp>().InSingletonScope();
            Bind<IEIPProductionIndexUsersLogin>().To<EIPProductionIndexUsersLogin>().InSingletonScope();
        }
    }
}
