using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.Widget
{
    [Flags]
    public enum RightEnum
    {
        None = 0,
        Read = 0x0001,
        Search = 0x0002,
        Delete = 0x0004,
        Create = 0x0008,
        Modify = 0X0010,
        Check = 0x0020
    }
}
