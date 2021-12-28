using EIPMonitor.Model.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.Model
{
    public class EIPProductionIndexUsers
    {
        public string UserName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string EmployeeId { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Status { get; set; }
        public String StatusName { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string newPassword { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string ClearTextPassword { get; set; }
    }
}
