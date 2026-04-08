using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using New_PRO.Models;
using New_PRO.Service;

namespace New_PRO.Testing
{
    [TestFixture]
    public class Login_Testing
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
        public async Task LoginAsync_UserNotFound_ReturnsFailure()
        {
            var login = new Login { Username = "unknown", Password = "123" };

            _authRepoMock
                .Setup(x => x.FindByUsernameAsync(login.Username))
                .ReturnsAsync((UserRegister)null);

            _authRepoMock
                .Setup(x => x.FindByEmailAsync(login.Username))
                .ReturnsAsync((UserRegister)null);

            var result = await _authService.LoginAsync(login);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("User not found"));
        }

        [Test]
        public async Task LoginAsync_InvalidPassword_ReturnsFailure()
        {
            var login = new Login { Username = "user1", Password = "wrong" };

            var user = new UserRegister { UserName = "user1" };

            _authRepoMock
                .Setup(x => x.FindByUsernameAsync(login.Username))
                .ReturnsAsync(user);

            _authRepoMock
                .Setup(x => x.CheckPasswordAsync(user, login.Password))
                .ReturnsAsync(false);

            var result = await _authService.LoginAsync(login);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Invalid password"));
        }

        [Test]
        public async Task LoginAsync_Success_ReturnsTokenAndRole()
        {
            var login = new Login { Username = "user1", Password = "correct" };

            var user = new UserRegister { UserName = "user1" };

            _authRepoMock
                .Setup(x => x.FindByUsernameAsync(login.Username))
                .ReturnsAsync(user);

            _authRepoMock
                .Setup(x => x.CheckPasswordAsync(user, login.Password))
                .ReturnsAsync(true);

            _authRepoMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin" });

            _jwtServiceMock
                .Setup(x => x.GenerateToken("user1", "Admin"))
                .Returns("jwt-token");

            var result = await _authService.LoginAsync(login);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Is.EqualTo("Login Successful"));
            Assert.That(result.Token, Is.EqualTo("jwt-token"));
            Assert.That(result.Role, Is.EqualTo("Admin"));
        }

        [Test]
        public async Task LoginAsync_NoRole_AssignsDefaultUserRole()
        {
            var login = new Login { Username = "user2", Password = "correct" };

            var user = new UserRegister { UserName = "user2" };

            _authRepoMock
                .Setup(x => x.FindByUsernameAsync(login.Username))
                .ReturnsAsync(user);

            _authRepoMock
                .Setup(x => x.CheckPasswordAsync(user, login.Password))
                .ReturnsAsync(true);

            _authRepoMock
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string>()); // no roles

            _jwtServiceMock
                .Setup(x => x.GenerateToken("user2", "User"))
                .Returns("token");

            var result = await _authService.LoginAsync(login);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Role, Is.EqualTo("User"));
        }
    }
}