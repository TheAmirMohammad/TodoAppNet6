using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoAppNet6.Data;
using TodoAppNet6.Models.Auth;
using TodoAppNet6.Servises.Auth;

namespace TodoAppNet6.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly UserManager<User> _userManager;

        public AuthController(TodoContext context,
                              IAuthenticationManager authenticationManager,
                              UserManager<User> userManager)
        {
            _context = context;
            _authenticationManager = authenticationManager;
            _userManager = userManager;
        }

        
    }
}
