using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.DataAcsess.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public string? DoctorId { get; set; }
        public string? PatientId { get; set; }
    }
}
