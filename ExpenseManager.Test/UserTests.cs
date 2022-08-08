using AutoMapper;
using ExpenseManager.ApplicationService.Commands.UserCommands;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Exceptions;
using ExpenseManager.ApplicationService.Utilities;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.Exceptions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Test
{
    public class UserTests
    {
        private readonly Mock<IMapper> _autoMapper;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IHttpContextService> _identityService;
        private readonly Mock<ISecurityHelper> _securityHelper;

        private const string samplePassword = "SamplePassword";
        private const string sampleUsername = "SampleUsername";
        public UserTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _autoMapper = new Mock<IMapper>();
            _identityService = new Mock<IHttpContextService>();
            _securityHelper = new Mock<ISecurityHelper>();
        }

        #region Domain
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateUser_ShouldThrowDomainException_WhenUsernameIsInvalid(string username)
        {
            Assert.Throws<ExpenseManagerDomainException>(() => User.Create(username, samplePassword));
        }

        [Fact]
        public void CreateUser_ShouldThrowDomainException_WhenUsernameIsNull()
        {
            Assert.Throws<ExpenseManagerDomainException>(() => User.Create(null, samplePassword));
        }

        [Fact]
        public void CreateUser_ShouldCorrectTheUsername_WhenItContainsWhiteSpace()
        {
            //Arrange
            string username = " SampleUsername ";
            string expectedUsername = username.Trim();

            //Act
            User user = User.Create(username, samplePassword);
            string actualUsername = user.Username;

            //Assert
            Assert.Equal(actualUsername, expectedUsername);
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateUser_ShouldThrowDomainException_WhenPasswordIsInvalid(string password)
        {
            Assert.Throws<ExpenseManagerDomainException>(() => User.Create(sampleUsername, password));
        }

        [Fact]
        public void CreateUser_ShouldThrowDomainException_WhenPasswordIsNull()
        {
            Assert.Throws<ExpenseManagerDomainException>(() => User.Create(sampleUsername, null));
        }

        [Fact]
        public void CreateUser_ShouldCorrectThePassword_WhenItContainsWhiteSpace()
        {
            string password = " SamplePassword ";
            string expectedPassword = password.Trim();

            User user = User.Create(sampleUsername, password);
            string actualPassword = user.PasswordHash;

            Assert.Equal(actualPassword, expectedPassword);
        }

        #endregion Domain

        #region ApplicationService
        [Fact]
        public async Task AddUser_ShouldThrowApplicationServiceException_WhenUsernameIsDuplicated()
        {
            var addCommand = new AddUserCommand() { Username = sampleUsername, Password = samplePassword };
            _userRepository.Setup(x => x.IsDuplicatedUsername(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var handler = new AddUserCommandHandler(_userRepository.Object, _autoMapper.Object, _securityHelper.Object);
            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("Username is duplicated", exception.Message);
        }

        [Fact]
        public async Task AddUser_ShouldWork_WhenUserPropertiesAreValid()
        {
            var addCommand = new AddUserCommand() { Username = sampleUsername, Password = samplePassword };
            var usersList = new List<User>();
            var expected = new UserDto() { Username = sampleUsername };
            _userRepository.Setup(x => x.IsDuplicatedUsername(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _userRepository.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                 .Callback((User user, CancellationToken token, bool saveNow) =>
                 {
                     usersList.Add(user);
                 });
            _securityHelper.Setup(x => x.GetSha256Hash(It.IsAny<string>())).Returns<string>(x => x);

            var handler = new AddUserCommandHandler(_userRepository.Object, _autoMapper.Object, _securityHelper.Object);

            UserDto actual = await handler.Handle(addCommand, new CancellationToken());
            Assert.True(usersList.Any());
            Assert.Equal(expected.Username, usersList.Last().Username);
            _userRepository.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task ChangeUserPassword_ShouldThrowApplicationServiceException_WhenUserNotFound()
        {
            var changeCommand = new ChangeUserPasswordCommand() {OldPassword= samplePassword, NewPassword = samplePassword, ConfirmNewPassword = samplePassword };
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(),It.IsAny<int>())).Returns(() => Task.FromResult<User>(null));
          
            var handler = new ChangeUserPasswordCommandHandler(_userRepository.Object, _identityService.Object, _securityHelper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(changeCommand, new CancellationToken()));
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task ChangeUserPassword_ShouldThrowApplicationServiceException_WhenOldPasswordIsWrong()
        {
            var actualOldPassword = samplePassword + "Test";
            var changeCommand = new ChangeUserPasswordCommand() { OldPassword = samplePassword, NewPassword = samplePassword, ConfirmNewPassword = samplePassword };
            User user = User.Create(sampleUsername, actualOldPassword);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).ReturnsAsync(() => user);
            
            var handler = new ChangeUserPasswordCommandHandler(_userRepository.Object, _identityService.Object, _securityHelper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(changeCommand, new CancellationToken()));
            Assert.Equal("OldPassword is not valid.", exception.Message);
        }

        [Fact]
        public async Task ChangeUserPassword_ShouldWork_WhenPropertiesAreValid()
        {
            var newPassword = samplePassword + "Test";
            var changeCommand = new ChangeUserPasswordCommand() { OldPassword = samplePassword, NewPassword = newPassword, ConfirmNewPassword = newPassword };
            User user = User.Create(sampleUsername, samplePassword);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).ReturnsAsync(() => user);
            _securityHelper.Setup(x => x.GetSha256Hash(It.IsAny<string>())).Returns<string>(x => x);

            var handler = new ChangeUserPasswordCommandHandler(_userRepository.Object, _identityService.Object, _securityHelper.Object);
            await handler.Handle(changeCommand, new CancellationToken());

            Assert.Equal(newPassword, user.PasswordHash);
            _userRepository.Verify(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Once);
        }
        #endregion ApplicationService
    }
}
