using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TodoAppNet6.Models.Auth;

namespace TodoAppNet6.Services.UserProp
{
    public class UserService : IUserInterface
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<User> _userManager;

        public UserService(IHttpContextAccessor httpContext, UserManager<User> userManager)
        {
            _httpContext = httpContext;
            _userManager = userManager;
        }
        public string? GetName()
        {
            var context = _httpContext.HttpContext;
            if (context == null)
                return null;
            return context.User.FindFirst(ClaimTypes.Name)!.Value;
        }
        
        public string? GetId()
        {
            var context = _httpContext.HttpContext;
            if (context == null)
                return null;
            return context.User.FindFirst(ClaimTypes.PrimarySid)!.Value;
        }

        public User? GetUser()
        {
            var username = GetName();
            if (username == null)
                return null;
            var user = _userManager.FindByNameAsync(username).Result;
            return user;
        }

        public User? GetUserById(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            return user;
        }
    }
}
