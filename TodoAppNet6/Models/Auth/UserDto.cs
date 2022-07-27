using System.ComponentModel.DataAnnotations;

namespace TodoAppNet6.Models.Auth
{
    public class UserDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public DateTime Birthdate { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? Email { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
    }
}
