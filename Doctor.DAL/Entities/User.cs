using Microsoft.AspNetCore.Identity;

namespace Doctor.DataAcsess.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? FatherName { get; set; }
        public string? Gender { get; set; }
        public DateTime? RegisteredOn { get; set; }
        public DateTime? LastActive { get; set; }
        public string? Introduction { get; set; }
        public string? Speciality { get; set; }
        public List<Message>? MessagesSent { get; set; }
        public List<Message>? MessagesReceived { get; set; }
    }
}
