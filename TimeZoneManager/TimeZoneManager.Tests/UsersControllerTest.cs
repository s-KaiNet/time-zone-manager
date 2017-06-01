using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeZoneManager.Services.Interfaces;
using Moq;
using TimeZoneManager.AutoMapper;
using TimeZoneManager.Controllers;
using TimeZoneManager.Services.Model;
using TimeZoneManager.ViewModel;
using ExtMapper = AutoMapper.Mapper;

namespace TimeZoneManager.Tests
{
    [TestClass]
    public class UsersControllerTest
    {
        private IMapper Mapper { get; }

        public UsersControllerTest()
        {
            ExtMapper.Initialize(config =>
            {
                config.AddProfile(typeof(MappingProfile));
            });

            Mapper = ExtMapper.Instance;
        }

        [TestMethod]
        public async Task CreateNewUser_WithValidUser_CreatesUserWithSucceseedState()
        {
            //Assert
            var newUserViewModel = new NewUserViewModel
            {
                Password = "sdf",
                LoginName = "sf",
                Email = "sf@g.com",
                DisplayName = "afff",
                Role = "ff"
            };

            var loggerMock = new Mock<ILogger<UsersController>>();
            var userServiceMock = new Mock<IUsersService>();
            userServiceMock.Setup(s => s.RegisterNewUser(It.IsAny<Register>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var usersController = new UsersController(loggerMock.Object, Mapper, userServiceMock.Object);

            //Act
            var result = await usersController.Create(newUserViewModel);

            //Assert
            Assert.IsTrue(result is IdentityResult);
            Assert.IsTrue((result as IdentityResult).Succeeded);
        }
    }
}
