using Microsoft.AspNetCore.Mvc;
using Moq;
using PhotoAlbum.Api.Authentication;
using PhotoAlbum.Api.Controllers;
using PhotoAlbum.Api.Repositories.Interfaces;
using PhotoAlbum.Shared.Enums;
using PhotoAlbum.Shared.Models;
using Xunit;

namespace PhotoAlbum.Test
{
    public class UserControllerTest
    {
        private JwtSettings _jwtSettings;

        public UserControllerTest() 
        {
             _jwtSettings = new JwtSettings
             {
                 Key = "1234567890",
                 TokenMinutes = 20
             };
        }

        [Fact]
        public async Task GetUsersAsync()
        {
            var data = new List<UserModel>
            {
                new() { UserId = 1, Username = "test", Name = "Demo Test", Password = "test", Role = RolesEnum.Admin.ToString() },
                new() { UserId = 2, Username = "admin", Name = "Demo Admin", Password = "admin", Role = RolesEnum.Admin.ToString() },
                new() { UserId = 3, Username = "user",  Name = "Demo User",  Password = "user",  Role = RolesEnum.User.ToString() }
            };

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetUsersAsync()).ReturnsAsync(data);

            var userController = new UserController(userRepository.Object, _jwtSettings);

            var actionResult = await userController.GetUsersAsync();
            var result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            var users = result.Value as List<UserModel>;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(users);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(users.Count == 3);
        }

        [Fact]
        public async Task GetUserAsync()
        {
            var data = new UserModel { UserId = 1, Username = "test", Name = "Demo Test", Password = "test", Role = RolesEnum.Admin.ToString() };

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetUserAsync(1)).ReturnsAsync(data);

            var userController = new UserController(userRepository.Object, _jwtSettings);

            // Get user 1
            var actionResult = await userController.GetUserAsync(1);
            var result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            var user = result.Value as UserModel;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(user);

            // Get user 0
            actionResult = await userController.GetUserAsync(0);
            result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(result);
        }

        [Fact]
        public async Task PasswordIsValidAsync()
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.PasswordIsValidAsync(1, "test")).ReturnsAsync(true);

            var userController = new UserController(userRepository.Object, _jwtSettings);

            var actionResult = await userController.ValidatePasswordAsync(1, "test");
            var result = actionResult.Result as OkObjectResult;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            var isValid = result.Value as bool?;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(isValid);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(isValid);
        }
    }
}
