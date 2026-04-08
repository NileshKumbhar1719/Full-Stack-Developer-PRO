using Microsoft.AspNetCore.Identity;
namespace New_PRO.Service
{
    public interface IJwtService
    {
        string GenerateToken(string username, string role);
    }
}
