using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using New_PRO.Models;
using New_PRO.Service;

namespace New_PRO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            var result = await _authService.RegisterAsync(register);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var result = await _authService.LoginAsync(login);
            if (result.IsSuccess) return Ok(result);
            return Unauthorized(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new { message = "Logout Successful" });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return Ok("Admin Only Endpoint");
        }

        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public IActionResult User()
        {
            return Ok("User Only Endpoint");
        }
    }
}