using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeZoneManager.Data.Context;
using TimeZoneManager.Data.Models;
using TimeZoneManager.Services.Common;
using TimeZoneManager.Services.Exceptions;
using TimeZoneManager.Services.Interfaces;
using TimeZoneManager.Services.Model;

// ReSharper disable once CheckNamespace
namespace TimeZoneManager.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<TimeZoneAppUser> _userManager;
        private readonly TimeZonesContext _context;

        public UsersService(UserManager<TimeZoneAppUser> userManager, TimeZonesContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> RegisterNewUser(Register model)
        {
            return await RegisterNewUser(model, Roles.USER);
        }

        public async Task<IdentityResult> RegisterNewUser(Register model, string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                role = Roles.USER;
            }

            var user = await _userManager.FindByNameAsync(model.LoginName);

            if (user != null)
            {
                throw new UserExistsException();
            }

            var result = await _userManager.CreateAsync(new TimeZoneAppUser
            {
                DisplayName = model.DisplayName,
                UserName = model.LoginName,
                Email = model.Email
            }, model.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            user = await _userManager.FindByNameAsync(model.LoginName);

            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> UpdateUser(Register model, string role)
        {
            var user = await _userManager.FindByNameAsync(model.LoginName);

            user.DisplayName = model.DisplayName;
            user.Email = model.Email;

            await _userManager.UpdateAsync(user);
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> DeleteUser(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return await _userManager.DeleteAsync(user);
        }

        public async Task<TimeZoneAppUser> FindByName(string name)
        {
            var user =  await _userManager.FindByNameAsync(name);
            var roles = await _userManager.GetRolesAsync(user);
            user.Role = roles.First();

            return user;
        }

        public IList<TimeZoneAppUser> GetAll(FilterQuery filter, out int total)
        {
            var result = _userManager.Users.Include(c => c.Roles).OrderBy(u => u.DisplayName).Page(filter.PageSize, filter.Page).ToList();
            var allRoleClaims = _context.RoleClaims.ToList();
            foreach (var user in result)
            {
                user.RoleClaims = new List<IdentityRoleClaim<string>>();
                foreach (var role in user.Roles)
                {
                    var roleClaims = allRoleClaims.Where(r => r.RoleId == role.RoleId);
                    user.RoleClaims.AddRange(roleClaims);
                }
            }

            total = _userManager.Users.Count();

            return result;
        }
    }
}