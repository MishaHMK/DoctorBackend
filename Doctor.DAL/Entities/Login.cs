using System.ComponentModel.DataAnnotations;

namespace Doctor.DataAcsess.Entities
{
    public class Login
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
