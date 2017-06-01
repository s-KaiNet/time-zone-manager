using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeZoneManager.Filters;
using TimeZoneManager.Services.Exceptions;
using TimeZoneManager.Services.Interfaces;
using TimeZoneManager.Services.Model;
using TimeZoneManager.ViewModel;

namespace TimeZoneManager.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [WebApiExceptionFilter]
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;

        public RegisterController(ILogger<RegisterController> logger, IMapper mapper, IUsersService usersService)
        {
            _logger = logger;
            _mapper = mapper;
            _usersService = usersService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<object> Register([FromBody]RegisterViewModel viewModel)
        {
            try
            {
                _logger.LogTrace("Register user");

                var model = _mapper.Map<Register>(viewModel);
                var result = await _usersService.RegisterNewUser(model);

                return result;
            }
            catch (UserExistsException)
            {
                return new { UserExists = true };
            }
        }
    }
}
