using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.DataAcsess.Entities
{
    public class UserRateDTO : IdentityUser
    {
        public string Name { get; set; }
        public string? Gender { get; set; }
        public DateTime? RegisteredOn { get; set; }
        public DateTime? LastActive { get; set; }
        public string? Introduction { get; set; }
        public string? Speciality { get; set; }
        public double? AverageRate { get; set; }    
    }
}
