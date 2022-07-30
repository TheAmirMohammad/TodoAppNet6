using System.ComponentModel.DataAnnotations;

namespace TodoAppNet6.Models.TodoItem
{
    public class TodoDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public bool IsDone { get; set; } = false;

        public string? UserId { get; set; }

        public Guid? FolderId { get; set; }
    }
}
