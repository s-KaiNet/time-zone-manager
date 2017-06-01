using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace TimeZoneManager.Filters
{
    public class WebApiExceptionFilterAttribute : TypeFilterAttribute
    {
        public WebApiExceptionFilterAttribute() : base(typeof(WebApiExceptionFilterImplAttribute))
        {
        }

        private class WebApiExceptionFilterImplAttribute : ExceptionFilterAttribute
        {
            private readonly ILogger _logger;

            public WebApiExceptionFilterImplAttribute()
            {
                _logger = LogManager.GetCurrentClassLogger();
            }

            public override void OnException(ExceptionContext context)
            {
               _logger.Error(context.Exception);

                var result = new
                {
                    StatusCode = 500,
                    Message = context.Exception.Message,
                    StackTrace = context.Exception.StackTrace,
                    InnerMessage = context.Exception.InnerException?.Message,
                    InnerStackTrace = context.Exception.InnerException?.StackTrace
                };

                context.Result = new JsonResult(result);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
