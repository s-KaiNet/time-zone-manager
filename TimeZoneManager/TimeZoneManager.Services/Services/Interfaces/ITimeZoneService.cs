using System;
using System.Collections.Generic;
using TimeZoneManager.Services.Common;
using TimeZone = TimeZoneManager.Data.Models.TimeZone;

namespace TimeZoneManager.Services.Services.Interfaces
{
    public interface ITimeZoneService
    {
        IList<TimeZone> GetUserTimeZones(string userId, FilterQuery filter, out int total);
        IList<TimeZone> GetAllTimeZones(FilterQuery filter, out int total);
        TimeZone AddTimeZone(TimeZone timeZone);
        TimeZone UpdateTimeZone(TimeZone timeZone);
        void DeleteTimeZone(Guid id);
    }
}
