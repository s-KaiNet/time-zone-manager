using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeZoneManager.Filters;
using TimeZoneManager.Services.Common;
using TimeZoneManager.Services.Services.Interfaces;
using TimeZoneManager.ViewModel;
using TimeZone = TimeZoneManager.Data.Models.TimeZone;

namespace TimeZoneManager.Controllers
{
    [Route("api/[controller]")]
    [WebApiExceptionFilter]
    [Authorize(Policy = "Users")]
    public class TimeZonesController : Controller
    {
        private readonly ILogger<TimeZonesController> _logger;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IMapper _mapper;

        public TimeZonesController(ILogger<TimeZonesController> logger, IMapper mapper, ITimeZoneService timeZoneService)
        {
            _logger = logger;
            _timeZoneService = timeZoneService;
            _mapper = mapper;
        }

        //GET api/timezones/{userId}
        [HttpGet("{userId}")]
        public TimeZonesViewModelData Get(string userId, [FromQuery]int page = 1, [FromQuery]int pageSize = 5, [FromQuery]string filter = "")
        {
            _logger.LogTrace("GET api/timezones/{userId}");
            int total;
            var viewModel = _mapper.Map<IList<TimeZoneViewModel>>(_timeZoneService.GetUserTimeZones(userId, new FilterQuery
            {
                PageSize = pageSize,
                Page = page,
                Filter = filter
            }, out total));

            return new TimeZonesViewModelData
            {
                TimeZones = PopulateUtcDate(viewModel),
                Total = total
            };
        }

        //GET api/timezones
        [HttpGet]
        [Authorize(Policy = "Admins")]
        public TimeZonesViewModelData Get([FromQuery]int page = 1, [FromQuery]int pageSize = 5, [FromQuery]string filter = "")
        {
            _logger.LogTrace("GET api/timezones");
            int total;
            var viewModel = _mapper.Map<IList<TimeZoneViewModel>>(_timeZoneService.GetAllTimeZones(new FilterQuery
            {
                PageSize = pageSize,
                Page = page,
                Filter = filter
            }, out total));

            return new TimeZonesViewModelData
            {
                TimeZones = PopulateUtcDate(viewModel),
                Total = total
            };
        }

        //POST api/timezones
        [HttpPost]
        [ValidateModel]
        public TimeZoneViewModel Create([FromBody]TimeZoneViewModel timeZoneViewModel)
        {
            _logger.LogTrace("POST api/timezones");
            var ownerId = User.Claims.Single(c => c.Type.Equals(CustomClaimTypes.UserId)).Value;

            var timeZone = _mapper.Map<TimeZone>(timeZoneViewModel);
            timeZone.OwnerId = ownerId;

            var timeZoneView = _mapper.Map<TimeZoneViewModel>(_timeZoneService.AddTimeZone(timeZone));
            timeZoneView.OffsetTime = UnixTicks(DateTime.UtcNow);

            return timeZoneView;
        }

        // PUT api/timezones/{id}
        [HttpPut("{id:guid}")]
        [ValidateModel]
        public TimeZoneViewModel Put(Guid id, [FromBody]TimeZoneViewModel timeZoneViewModel)
        {
            _logger.LogTrace("PUT api/timezones/{id}");
            var timeZone = _mapper.Map<TimeZone>(timeZoneViewModel);
            timeZone.Id = id;

            var timeZoneView = _mapper.Map <TimeZoneViewModel>(_timeZoneService.UpdateTimeZone(timeZone));
            timeZoneView.OffsetTime = UnixTicks(DateTime.UtcNow);

            return timeZoneView;
        }

        //DELETE api/timezones/{id}
        [HttpDelete("{id}")]
        public StatusCodeResult Delete(Guid id)
        {
            _logger.LogTrace("DELETE api/timezones/{id}");
            _timeZoneService.DeleteTimeZone(id);

            return Ok();
        }

        private IList<TimeZoneViewModel> PopulateUtcDate(IList<TimeZoneViewModel> model)
        {
            var date = DateTime.UtcNow;
            foreach (var timeZone in model)
            {
                timeZone.OffsetTime = UnixTicks(date);
            }

            return model;
        }

        public static double UnixTicks(DateTime dt)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = dt.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }
    }
}
