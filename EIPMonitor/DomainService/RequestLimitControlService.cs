using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIPMonitor.DomainService
{
    public class RequestLimitControlService : IRequestLimitControlService
    {
        private ConcurrentDictionary<String, Int32> clickCounter = new ConcurrentDictionary<string, int>();
        private static object obj = new object();
        public RequestLimitControlService() { }
        public bool RequestClickPermission(string className, string controlName)
        {
            var key = $"{className}{controlName}";
            var ifAddSuccessFully = clickCounter.TryAdd(key, 1);
            if (ifAddSuccessFully) return true;
            Int32 originalValue;
            lock (obj)
            {
                while (!clickCounter.TryGetValue(key, out originalValue)) ;
                Int32 newValue = originalValue + 1;
                while (!clickCounter.TryUpdate(key, newValue, originalValue)) ;
            }
            return originalValue == 0;
        }

        public void ReleaseClickPermission(string className, string controlName)
        {
            var key = $"{className}{controlName}";
            var ifAddSuccessFully = clickCounter.TryAdd(key, 0);
            if (ifAddSuccessFully) return;
            lock (obj)
            {
                Int32 originalValue;
                while (!clickCounter.TryGetValue(key, out originalValue)) ;
                while (!clickCounter.TryUpdate(key, 0, originalValue)) ;
            }
        }
    }
}
