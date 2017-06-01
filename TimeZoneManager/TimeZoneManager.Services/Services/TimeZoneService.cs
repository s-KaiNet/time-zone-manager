using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TimeZoneManager.Data.Context;
using TimeZoneManager.Services.Common;
using TimeZoneManager.Services.Services.Interfaces;
using TimeZone = TimeZoneManager.Data.Models.TimeZone;

namespace TimeZoneManager.Services.Services
{
    public class TimeZoneService : ITimeZoneService
    {
        private readonly TimeZonesContext _context;

        public TimeZoneService(TimeZonesContext context)
        {
            _context = context;
        }

        public IList<TimeZone> GetUserTimeZones(string userId, FilterQuery filter, out int total)
        {
            var timeZoneQuery = string.IsNullOrEmpty(filter.Filter)
                ? _context.TimeZones.Include(c => c.Owner).Where(c => c.OwnerId == userId)
                : _context.TimeZones.Include(c => c.Owner).Where(c => c.OwnerId == userId && c.Name.Contains(filter.Filter));

            total = timeZoneQuery.Count();
            var result = timeZoneQuery.OrderBy(z => z.Name).Page(filter.PageSize, filter.Page).ToList();

            return result;
        }

        public IList<TimeZone> GetAllTimeZones(FilterQuery filter, out int total)
        {
            var timeZoneQuery = string.IsNullOrEmpty(filter.Filter)
                ? _context.TimeZones.Include(c => c.Owner)
                : _context.TimeZones.Include(c => c.Owner).Where(c => c.Name.Contains(filter.Filter));

            total = timeZoneQuery.Count();
            var result = timeZoneQuery.OrderBy(z => z.Name).Page(filter.PageSize, filter.Page).ToList();

            return result;
        }

        public TimeZone AddTimeZone(TimeZone timeZone)
        {
            var entry = _context.TimeZones.Add(timeZone);
            _context.SaveChanges();

            return entry.Entity;
        }

        public TimeZone UpdateTimeZone(TimeZone timeZone)
        {
            var entry = _context.TimeZones.Single(t => t.Id.Equals(timeZone.Id));

            entry.Name = timeZone.Name;
            entry.City = timeZone.City;
            entry.Offset = timeZone.Offset;

            _context.TimeZones.Update(entry);
            _context.SaveChanges();

            return entry;
        }

        public void DeleteTimeZone(Guid id)
        {
            var entry = _context.TimeZones.SingleOrDefault(t => t.Id.Equals(id));
            if (entry == null)
            {
                return;
            }
            _context.TimeZones.Remove(entry);
            _context.SaveChanges();
        }
    }
}

