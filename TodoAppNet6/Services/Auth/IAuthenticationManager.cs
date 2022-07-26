using TodoAppNet6.Models.Auth;

namespace TodoAppNet6.Servises.Auth
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateCreds(UserCreds creds);
    }
}
