using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoAppNet6.Models.Auth;
using TodoAppNet6.Models.TodoItem;

namespace TodoAppNet6.Models.Folders
{
    public class Folder
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;
        public virtual User? User { get; set; }

        public virtual ICollection<Todo>? Todoes { get; set; }
    }
}
