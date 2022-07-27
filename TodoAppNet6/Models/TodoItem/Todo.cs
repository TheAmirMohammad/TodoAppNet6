using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoAppNet6.Models.Auth;

namespace TodoAppNet6.Models.TodoItem
{
    public class Todo
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsDone { get; set; } = false;

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
