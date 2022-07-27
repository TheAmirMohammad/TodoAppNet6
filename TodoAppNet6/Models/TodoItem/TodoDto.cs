using System.ComponentModel.DataAnnotations;

namespace TodoAppNet6.Models.TodoItem
{
    public class TodoDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsDone { get; set; } = false;

        public string? UserId { get; set; }
    }
}
