using Microsoft.AspNetCore.Identity;
using New_PRO.Models;

public interface IAuthRepository
{
    Task<IdentityResult> CreateUserAsync(UserRegister user, string password);
    Task<UserRegister> FindByUsernameAsync(string username);
    Task<UserRegister> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(UserRegister user, string password);
    Task<bool> RoleExistsAsync(string role);
    Task<IdentityResult> CreateRoleAsync(string role);
    Task<IdentityResult> AddToRoleAsync(UserRegister user, string role);
    Task SignOutAsync();
    Task<IList<string>> GetRolesAsync(UserRegister user);
}