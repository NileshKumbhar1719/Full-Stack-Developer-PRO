using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using New_PRO.Models;
using New_PRO.Service;

namespace New_PRO.Testing
{
    [TestFixture]
    public class Register_Testing
    {
        private Mock<IAuthRepository> _authRepoMock;
        private Mock<IJwtService> _jwtServiceMock;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _authRepoMock = new Mock<IAuthRepository>();
            _jwtServiceMock = new Mock<IJwtService>();

            _authService = new AuthService(_authRepoMock.Object, _jwtServiceMock.Object);
        }

        [Test]
        public async Task RegisterAsync_UserAlreadyExists_ReturnsFailure()
        {
            var register = new Register { UserName = "testuser" };

            _authRepoMock
                .Setup(x => x.FindByUsernameAsync(register.UserName))
                .ReturnsAsync(new UserRegister());

            var result = await _authService.RegisterAsync(register);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("User already exists"));
        }

        [Test]
        public async Task RegisterAsync_Success_ReturnsToken()
        {
            var register = new Register
            {
                UserName = "newuser",
                Password = "Password123",
                Role = "Admin"
            };

            _authRepoMock
                .Setup(x => x.FindByUsernameAsync(register.UserName))
                .ReturnsAsync((UserRegister)null);

            _authRepoMock
                .Setup(x => x.CreateUserAsync(It.IsAny<UserRegister>(), register.Password))
                .ReturnsAsync(IdentityResult.Success);

            _authRepoMock
                .Setup(x => x.RoleExistsAsync("Admin"))
                .ReturnsAsync(true);

            _jwtServiceMock
                .Setup(x => x.GenerateToken(register.UserName, "Admin"))
                .Returns("fake-token");

            var result = await _authService.RegisterAsync(register);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Token, Is.EqualTo("fake-token"));
            Assert.That(result.Role, Is.EqualTo("Admin"));
        }

        [Test]
        public async Task RegisterAsync_RoleNotExists_CreatesRole()
        {
            var register = new Register
            {
                UserName = "user2",
                Password = "Password123",
                Role = "Manager"
            };

            _authRepoMock
                .Setup(x => x.FindByUsernameAsync(register.UserName))
                .ReturnsAsync((UserRegister)null);

            _authRepoMock
                .Setup(x => x.CreateUserAsync(It.IsAny<UserRegister>(), register.Password))
                .ReturnsAsync(IdentityResult.Success);

            _authRepoMock
                .Setup(x => x.RoleExistsAsync("Manager"))
                .ReturnsAsync(false);

            _jwtServiceMock
                .Setup(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("token");

            var result = await _authService.RegisterAsync(register);

            _authRepoMock.Verify(x => x.CreateRoleAsync("Manager"), Times.Once);
            Assert.That(result.IsSuccess, Is.True);
        }
    }
}