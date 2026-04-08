using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using New_PRO.Service;

namespace New_PRO.Testing
{
    [TestFixture]
    public class Logout_Testing
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
        public async Task LogoutAsync_Should_Call_SignOutAsync_Once()
        {
            // Arrange
            _authRepoMock
                .Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _authService.LogoutAsync();

            // Assert
            _authRepoMock.Verify(x => x.SignOutAsync(), Times.Once);
        }
    }
}