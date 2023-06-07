using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.DataAcsess.Entities
{
    public class DoctorUser
    {
        public int Id { get; set; }
        public string? Introduction { get; set; }
        public string? Speciality { get; set; }
        public int? OfficeNumber { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
