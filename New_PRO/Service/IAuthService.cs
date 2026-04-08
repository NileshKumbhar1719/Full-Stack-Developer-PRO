using New_PRO.Models;

namespace New_PRO.Service
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(Register register);
        Task<AuthResponse> LoginAsync(Login login);
        Task LogoutAsync();
    }
}