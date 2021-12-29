using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace EIPMonitor.LocalInfrastructure
{
    public static class IocKernel
    {
        private readonly static IUnityContainer container = new UnityContainer();
        public static IUnityContainer UnityContainer { get => container; }
        public static T Get<T>()
        {
            return container.Resolve<T>();
        }
        public static T Get<T>( string name)
        {
            return container.Resolve<T>(name);
        }
    }
}
