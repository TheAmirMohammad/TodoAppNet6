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

        [HttpGet("me"), Authorize]
        public ActionResult<string> getMe()
        {
            var user = User.Identity?.Name;
            return Ok(new
            {
                username = user
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Check your data!");

            var ph = new PasswordHasher<User>();
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                NormalizedUserName = request.Username.ToUpper(),
                Email = request.Email,
                NormalizedEmail = request.Email?.ToUpper(),
                PhoneNumber = request.PhoneNumber,
                Birthdate = request.Birthdate,
            };
            user.PasswordHash = ph.HashPassword(user, request.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserCreds request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Check your data!");

            return !await _authenticationManager.ValidateCreds(request)
                ? Unauthorized("Check your credentials!")
                : Ok(new { token = await _authenticationManager.CreateToken() });
        }

        [HttpPost("addRole")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IdentityRole>> addRole(IdentityRole request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Check your data!");

            await _context.Roles.AddAsync(request);
            await _context.SaveChangesAsync();

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return request;
        }

        [HttpPost("assignRole")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IdentityUserRole<string>>> AssignRole(IdentityUserRole<string> request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _context.UserRoles.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}
