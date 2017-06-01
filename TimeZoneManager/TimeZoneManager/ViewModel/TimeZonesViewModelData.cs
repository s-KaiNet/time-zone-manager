using System.Collections.Generic;

namespace TimeZoneManager.ViewModel
{
    public class TimeZonesViewModelData
    {
        public IList<TimeZoneViewModel> TimeZones { get; set; }
        public int Total { get; set; }
    }
}
