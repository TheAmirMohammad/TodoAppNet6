using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoAppNet6.Models.Auth;
using TodoAppNet6.Models.TodoItem;

namespace TodoAppNet6.Data
{
    public class TodoContext: IdentityDbContext<User>
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }

        public DbSet<Todo>? Todo { get; set; }

    }
}
