using EIPMonitor.Model.Widget;
using Infrastructure.Standard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.SecurityServices.RightEnumServices
{
    public static class RightEnumCombobxService
    {
        public static List<ComboboxChecked> GetRights()
        {
            var result = Enum.GetValues(typeof(RightEnum)).Cast<RightEnum>().ToList().ConvertAll(r => new ComboboxChecked() { Name = r.ToString(), Check_Status = false });
            return result;
        }

    }
}
