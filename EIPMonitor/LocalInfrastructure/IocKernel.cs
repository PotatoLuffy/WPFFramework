using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.LocalInfrastructure
{
    public static class IocKernel
    {
        private static StandardKernel _kernel;
        public static T Get<T>()
        {
            return _kernel.Get<T>();
        }

        public static void Initialize(params INinjectModule[] moudules)
        {
            if (_kernel == null)
            {
                _kernel = new StandardKernel(moudules);   
            }
        }
    }
}
