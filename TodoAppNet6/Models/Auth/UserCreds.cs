using System.ComponentModel.DataAnnotations;

namespace TodoAppNet6.Models.Auth
{
    public class UserCreds
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
