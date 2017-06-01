using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TimeZoneManager.ViewModel
{
    public class UsersViewModel
    {
        public string DisplayName { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public List<IdentityRoleClaim<string>> RoleClaims { get; set; }
        public string Role { get; set; }
    }
}
