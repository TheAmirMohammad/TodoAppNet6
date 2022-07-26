using System.ComponentModel.DataAnnotations;

namespace TodoAppNet6.Models.Auth
{
    public class RoleDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
