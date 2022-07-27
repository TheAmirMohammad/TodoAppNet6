using TodoAppNet6.Models.Auth;

namespace TodoAppNet6.Services.UserProp
{
    public interface IUserInterface
    {
        string? GetName();
        Task<User?> GetUser();
    }
}
