using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoAppNet6.Data;
using TodoAppNet6.Models.Auth;

namespace TodoAppNet6.Services
{
    public static class ServicesExtentions
    {
        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequiredLength = 8;
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<TodoContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoContext>(
                o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
