using System.ComponentModel.DataAnnotations;
using TodoAppNet6.Models.TodoItem;

namespace TodoAppNet6.Models.Folders
{
    public class FolderDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

    }
}
