using Microsoft.AspNetCore.Identity;
using New_PRO.Models;

namespace New_PRO.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<UserRegister> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<UserRegister> _signInManager;

        public AuthRepository(UserManager<UserRegister> userManager,
                              RoleManager<IdentityRole> roleManager,
                              SignInManager<UserRegister> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(UserRegister user, string password)
            => await _userManager.CreateAsync(user, password);

        public async Task<UserRegister> FindByUsernameAsync(string username)
            => await _userManager.FindByNameAsync(username);

        public async Task<UserRegister> FindByEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<bool> CheckPasswordAsync(UserRegister user, string password)
            => await _userManager.CheckPasswordAsync(user, password);

        public async Task<bool> RoleExistsAsync(string role)
            => await _roleManager.RoleExistsAsync(role);

        public async Task<IdentityResult> CreateRoleAsync(string role)
            => await _roleManager.CreateAsync(new IdentityRole(role));

        public async Task<IdentityResult> AddToRoleAsync(UserRegister user, string role)
            => await _userManager.AddToRoleAsync(user, role);

        public async Task SignOutAsync()
            => await _signInManager.SignOutAsync();

        public async Task<IList<string>> GetRolesAsync(UserRegister user)
            => await _userManager.GetRolesAsync(user);
    }
}