using New_PRO.Models;
using New_PRO.Repository;

namespace New_PRO.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly IJwtService _jwtService;

        public AuthService(IAuthRepository authRepo, IJwtService jwtService)
        {
            _authRepo = authRepo;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> RegisterAsync(Register register)
        {
            var existingUser = await _authRepo.FindByUsernameAsync(register.UserName);
            if (existingUser != null)
                return new AuthResponse { IsSuccess = false, Message = "User already exists" };

            var user = new UserRegister
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.UserName,
                Email = register.Email
            };

            var result = await _authRepo.CreateUserAsync(user, register.Password);
            if (!result.Succeeded)
                return new AuthResponse { IsSuccess = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

            string role = string.IsNullOrWhiteSpace(register.Role) ? "User" : register.Role.Trim();

            if (!await _authRepo.RoleExistsAsync(role))
                await _authRepo.CreateRoleAsync(role);

            await _authRepo.AddToRoleAsync(user, role);

            var token = _jwtService.GenerateToken(user.UserName, role);

            return new AuthResponse { IsSuccess = true, Message = "User Registered Successfully", Token = token, Role = role };
        }

        public async Task<AuthResponse> LoginAsync(Login login)
        {
            var user = await _authRepo.FindByUsernameAsync(login.Username) ??
                       await _authRepo.FindByEmailAsync(login.Username);

            if (user == null)
                return new AuthResponse { IsSuccess = false, Message = "User not found" };

            if (!await _authRepo.CheckPasswordAsync(user, login.Password))
                return new AuthResponse { IsSuccess = false, Message = "Invalid password" };

            var roles = await _authRepo.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "User";

            var token = _jwtService.GenerateToken(user.UserName, role);

            return new AuthResponse { IsSuccess = true, Message = "Login Successful", Token = token, Role = role };
        }

        public async Task LogoutAsync()
        {
            await _authRepo.SignOutAsync();
        }
    }
}