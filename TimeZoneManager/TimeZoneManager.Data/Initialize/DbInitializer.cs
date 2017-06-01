using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TimeZoneManager.Data.Context;
using TimeZoneManager.Data.Models;

namespace TimeZoneManager.Data.Initialize
{
    public class DbInitializer
    {
        public static async Task Initialize(TimeZonesContext context, UserManager<TimeZoneAppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

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


            var adminUser = new TimeZoneAppUser { UserName = "admin", Email = "admin@timezones.com", DisplayName = "Administrator" };
            await userManager.CreateAsync(adminUser, "P@ssw0rd");

            if (!await userManager.IsInRoleAsync(adminUser, adminRole.Name))
            {
                await userManager.AddToRoleAsync(adminUser, adminRole.Name);
            }

            var user = new TimeZoneAppUser { UserName = "user", Email = "user@timezones.com", DisplayName = "Simple user" };
            await userManager.CreateAsync(user, "P@ssw0rd");

            if (!await userManager.IsInRoleAsync(user, userRole.Name))
            {
                await userManager.AddToRoleAsync(user, userRole.Name);
            }

            var manager = new TimeZoneAppUser { UserName = "manager", Email = "manager@timezones.com", DisplayName = "Manager user" };
            await userManager.CreateAsync(manager, "P@ssw0rd");

            if (!await userManager.IsInRoleAsync(manager, managerRole.Name))
            {
                await userManager.AddToRoleAsync(manager, managerRole.Name);
            }
        }
    }
}