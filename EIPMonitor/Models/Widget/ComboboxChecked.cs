using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model.Widget
{
    public class ComboboxChecked:IEquatable<ComboboxChecked>
    {
        public String Name { get; set; }
        public Boolean Check_Status { get; set; }
        public bool Equals(ComboboxChecked other)
        {
            if (other == null) return false;
            if (this.Name.Equals(other.Name)) return true;
            return false;
        }
        public override bool Equals(object obj)
        {
            ComboboxChecked other = obj as ComboboxChecked;
            return this.Equals(other);
        }
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
