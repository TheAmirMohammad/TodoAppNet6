using Microsoft.AspNetCore.Identity;

namespace TodoAppNet6.Models.Auth
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime Birthdate { get; set; }
    }
}
