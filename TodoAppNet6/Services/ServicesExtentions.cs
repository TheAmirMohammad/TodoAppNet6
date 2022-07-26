using Microsoft.EntityFrameworkCore;
using TodoAppNet6.Data;

namespace TodoAppNet6.Services
{
    public static class ServicesExtentions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoContext>(
                o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
