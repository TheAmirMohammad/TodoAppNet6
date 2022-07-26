using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoAppNet6.Models.Auth;

namespace TodoAppNet6.Servises.Auth
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User? _user;

        public AuthenticationManager(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _user = null;
        }

        public async Task<bool> ValidateCreds(UserCreds creds)
        {
            _user = await _userManager.FindByNameAsync(creds.Username);
            return _user != null && await _userManager.CheckPasswordAsync(_user, creds.Password);
        }
    }
}
