using System.ComponentModel.DataAnnotations;

namespace Doctor.DataAcsess.Entities
{
    public class Doctor
    {
        public string Id { get; set; }
        //public int Id { get; set; }
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? Fathername { get; set; }
        public string? Introduction { get; set; }
        public string? Speciality { get; set; }
        public int? OfficeNumber { get; set; }
    }
}
