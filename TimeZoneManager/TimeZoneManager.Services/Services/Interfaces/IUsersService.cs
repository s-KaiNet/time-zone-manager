using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TimeZoneManager.Data.Models;
using TimeZoneManager.Services.Common;
using TimeZoneManager.Services.Model;

// ReSharper disable once CheckNamespace
namespace TimeZoneManager.Services.Interfaces
{
    public interface IUsersService
    {
        Task<IdentityResult> RegisterNewUser(Register model);
        Task<IdentityResult> RegisterNewUser(Register model, string role);
        Task<IdentityResult> UpdateUser(Register model, string role);
        Task<IdentityResult> DeleteUser(string name);
        Task<TimeZoneAppUser> FindByName(string name);
        IList<TimeZoneAppUser> GetAll(FilterQuery filter, out int total);
    }
}