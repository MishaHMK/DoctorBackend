namespace Doctor.DataAcsess.Entities
{
    public class AppointmentDTOPage
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Duration { get; set; }
        public string? DoctorId { get; set; }
        public string? PatientId { get; set; }
        public bool IsApproved { get; set; }
        public string? AdminId { get; set; }
        public string? DoctorSurname { get; set; }
        public string? PatientSurname { get; set; }
        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorFathername { get; set; }
        public string? PatientFathername { get; set; }
        public int? OfficeNumber { get; set; }
    }
}
