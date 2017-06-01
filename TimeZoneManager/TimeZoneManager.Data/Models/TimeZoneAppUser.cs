using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TimeZoneManager.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class TimeZoneAppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public List<TimeZone> TimeZones { get; set; }

        [NotMapped]
        public List<IdentityRoleClaim<string>> RoleClaims { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
