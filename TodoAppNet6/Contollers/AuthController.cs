using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoAppNet6.Data;
using TodoAppNet6.Models.Auth;
using TodoAppNet6.Services.UserProp;
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
        private readonly IUserService _userInterface;

        public AuthController(TodoContext context,
                              IAuthenticationManager authenticationManager,
                              UserManager<User> userManager,
                              IUserService userInterface)
        {
            _context = context;
            _authenticationManager = authenticationManager;
            _userManager = userManager;
            _userInterface = userInterface;
        }

        [HttpGet("me"), Authorize]
        public ActionResult<string> getMe()
        {
            var username = _userInterface.GetName();
            var userId = _userInterface.GetId();
            return Ok(new
            {
                username = username,
                userId = userId
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
        public async Task<ActionResult<IdentityRole>> addRole(RoleDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Check your data!");

            var role = new IdentityRole
            {
                Id = request.Name.Trim() + "_role",
                Name = request.Name,
                NormalizedName = request.Name.ToUpper()
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return role;
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

        [HttpPost("unassignRole")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<string>> UnassignRole(IdentityUserRole<string> request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _context.UserRoles.Remove(request);
            await _context.SaveChangesAsync();
            return Ok("Role removd!");
        }
    }
}
