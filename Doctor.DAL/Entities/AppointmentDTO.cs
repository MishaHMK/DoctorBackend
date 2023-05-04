using System.ComponentModel.DataAnnotations;

namespace Doctor.DataAcsess.Entities
{
    public class AppointmentDTO
    {
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
        public int? Duration { get; set; }
        public string? DoctorId { get; set; }
        public string? PatientId { get; set; }
        public bool IsApproved { get; set; }
        public string? AdminId { get; set; }
        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }
    }
}
