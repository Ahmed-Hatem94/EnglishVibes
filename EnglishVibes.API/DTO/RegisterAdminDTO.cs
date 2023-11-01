using System.ComponentModel.DataAnnotations;

namespace EnglishVibes.API.DTO
{
    public class RegisterAdminDTO
    {
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}
