using System;
using System.ComponentModel.DataAnnotations;

namespace TimeZoneManager.ViewModel
{
    public class TimeZoneViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int Offset { get; set; }

        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public Guid Id { get; set; }
        public double OffsetTime { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
