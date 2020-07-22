using System;
using System.Threading.Tasks;
using AutoMapper;
using Edapp.Controllers;
using Edapp.Data;
using Edapp.Model.Response;
using Edapp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace edapp.tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task GetUserAsyncReturnsUserResponse()
        {
            var mockLogger = new Mock<ILogger<UserController>>();
            var user = GetMockUser();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(user));
            var userController = new UserController(mockLogger.Object, userRepository.Object);
            var controller = await userController.GetUserAsync(1);
            var actionResult = Assert.IsType<OkObjectResult>(controller.Result);
            var result = actionResult.Value as UserResponse;

            Assert.NotNull(result);
            Assert.Equal("Ralph", result.Name);
            Assert.Equal("ralph@gmail.com", result.Email);
            Assert.Equal("+61412367895", result.Phone);
        }

        [Fact]
        public async Task GetUserAsyncThrowsNotFoundException()
        {
            var mockLogger = new Mock<ILogger<UserController>>();
            UserResponse userResponse = null;
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(u => u.GetUserAsync(It.IsAny<int>())).Returns(Task.FromResult(userResponse));
            var userController = new UserController(mockLogger.Object, userRepository.Object);
            var controller = await userController.GetUserAsync(2);
            var result = Assert.IsType<NotFoundResult>(controller.Result);
            Assert.Equal(404, result.StatusCode);
        } 

        private UserResponse GetMockUser()
        {
            var user = new UserResponse();
            user.Id = 1;
            user.Name = "Ralph";
            user.Email = "ralph@gmail.com";
            user.Phone = "+61412367895";

            return user;
        }
    }
}
