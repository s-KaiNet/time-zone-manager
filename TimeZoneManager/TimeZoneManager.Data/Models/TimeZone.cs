using System;
using System.ComponentModel.DataAnnotations;

namespace TimeZoneManager.Data.Models
{
    public class TimeZone
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int Offset { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        [Key]
        public Guid Id { get; set; }

        public TimeZoneAppUser Owner { get; set; }
    }
}
