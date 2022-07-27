using TodoAppNet6.Models.Auth;

namespace TodoAppNet6.Services.UserProp
{
    public interface IUserInterface
    {
        string? GetName();
        string? GetId();
        Task<User?> GetUser();
    }
}
