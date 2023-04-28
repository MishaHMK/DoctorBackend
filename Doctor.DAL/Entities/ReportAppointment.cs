using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.DataAcsess.Entities
{
    public class ReportAppointment
    {
        public int? Id { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
