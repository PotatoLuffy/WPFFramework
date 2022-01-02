using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.ViewModel.CustomException
{
    public class LoginAuthenticatedFailureException:Exception
    {
        public LoginAuthenticatedFailureException(String msg) : base(msg) { }
        public LoginAuthenticatedFailureException() { }
    }
}
