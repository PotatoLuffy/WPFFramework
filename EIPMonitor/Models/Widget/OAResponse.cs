using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.Widget
{
    public class OAResponse
    {
        public bool SUCCESS { get; set; }
        public string MSG { get; set; }
        public string APPROVER { get; set; }
        public string unid { get; set; }
        public string sn { get; set; }
    }
}
