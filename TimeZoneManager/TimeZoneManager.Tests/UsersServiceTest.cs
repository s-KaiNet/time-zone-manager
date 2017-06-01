using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeZoneManager.Data.Context;
using TimeZoneManager.Data.Models;
using TimeZoneManager.Services;
using TimeZoneManager.Services.Common;
using TimeZoneManager.Services.Model;

namespace TimeZoneManager.Tests
{
    [TestClass]
    public class UsersServiceTest
    {
        private TimeZonesContext Context { get; }
        private UserManager<TimeZoneAppUser> UserManager { get; }

        public UsersServiceTest()
        {
            var services = new ServiceCollection();
            services.AddEntityFramework()
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<TimeZonesContext>(options => options.UseInMemoryDatabase());

            services.AddIdentity<TimeZoneAppUser, IdentityRole>()
                .AddEntityFrameworkStores<TimeZonesContext>();

            var context = new DefaultHttpContext();
            context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature());
            services.AddSingleton<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = context });
            var serviceProvider = services.BuildServiceProvider();

            Context = serviceProvider.GetRequiredService<TimeZonesContext>();
            UserManager = serviceProvider.GetRequiredService<UserManager<TimeZoneAppUser>>();

            SetupRoles(serviceProvider.GetRequiredService<RoleManager<IdentityRole>>());
        }

        [TestMethod]
        public async Task RegisterUser_WithValidModel_CreatesUser()
        {
            //Arrange
            var service = new UsersService(UserManager, Context);
            var registerModel = new Register
            {
                Email = "t.t@g.com",
                Password = "P@ssw0rd",
                LoginName = "test1",
                DisplayName = "test"
            };

            //Act
            await service.RegisterNewUser(registerModel);

            //Assert
            var user = await UserManager.FindByNameAsync(registerModel.LoginName);

            Assert.IsNotNull(user);
            Assert.AreEqual(user.Email, registerModel.Email);
        }

        [TestMethod]
        public async Task RegisterUser_WithEmptyRole_AssignesUserRole()
        {
            //Arrange
            var service = new UsersService(UserManager, Context);
            var registerModel = new Register
            {
                Email = "t.t@g.com",
                Password = "P@ssw0rd",
                LoginName = "test2",
                DisplayName = "test"
            };

            //Act
            await service.RegisterNewUser(registerModel, null);

            //Assert
            var user = await UserManager.FindByNameAsync(registerModel.LoginName);
            var role = (await UserManager.GetRolesAsync(user)).First();

            Assert.AreEqual(Roles.USER, role);
        }

        [TestMethod]
        public async Task RegisterUser_WithManagerRole_AssignesManagerRole()
        {
            //Arrange
            var service = new UsersService(UserManager, Context);
            var registerModel = new Register
            {
                Email = "t.t@g.com",
                Password = "P@ssw0rd",
                LoginName = "test3",
                DisplayName = "test"
            };

            //Act
            await service.RegisterNewUser(registerModel, Roles.MANAGER);

            //Assert
            var user = await UserManager.FindByNameAsync(registerModel.LoginName);
            var role = (await UserManager.GetRolesAsync(user)).First();

            Assert.AreEqual(Roles.MANAGER, role);
        }

        [TestMethod]
        public async Task RegisterUser_WithNotValidPassword_ReturnsErrorResponse()
        {
            //Arrange
            var service = new UsersService(UserManager, Context);
            var registerModel = new Register
            {
                Email = "t.t@g.com",
                Password = "123",
                LoginName = "test4",
                DisplayName = "test"
            };

            //Act
            var result = await service.RegisterNewUser(registerModel, Roles.MANAGER);

            //Assert
            Assert.IsTrue(!result.Succeeded);
        }

        private async void SetupRoles(RoleManager<IdentityRole> roleManager)
        {
            var userRole = new IdentityRole("user");
            await roleManager.CreateAsync(userRole);
            await roleManager.AddClaimAsync(userRole, new Claim(ClaimTypes.Role, userRole.Name));

            var managerRole = new IdentityRole("manager");
            await roleManager.CreateAsync(managerRole);
            await roleManager.AddClaimAsync(managerRole, new Claim(ClaimTypes.Role, managerRole.Name));
            await roleManager.AddClaimAsync(managerRole, new Claim(ClaimTypes.Role, userRole.Name));

            var adminRole = new IdentityRole("admin");
            await roleManager.CreateAsync(adminRole);
            await roleManager.AddClaimAsync(adminRole, new Claim(ClaimTypes.Role, adminRole.Name));
            await roleManager.AddClaimAsync(adminRole, new Claim(ClaimTypes.Role, managerRole.Name));
            await roleManager.AddClaimAsync(adminRole, new Claim(ClaimTypes.Role, userRole.Name));
        }
    }
}
