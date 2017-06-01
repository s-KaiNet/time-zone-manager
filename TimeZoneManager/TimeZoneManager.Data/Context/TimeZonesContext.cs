using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeZoneManager.Data.Models;

namespace TimeZoneManager.Data.Context
{
    public class TimeZonesContext : IdentityDbContext<TimeZoneAppUser>
    {
        public DbSet<TimeZone> TimeZones { get; set; }

        public TimeZonesContext(DbContextOptions<TimeZonesContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TimeZone>()
                .HasOne(p => p.Owner)
                .WithMany(p => p.TimeZones)
                .HasForeignKey(p => p.OwnerId);
        }
    }
}
