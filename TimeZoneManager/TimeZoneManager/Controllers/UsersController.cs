using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeZoneManager.Filters;
using TimeZoneManager.Services.Common;
using TimeZoneManager.Services.Exceptions;
using TimeZoneManager.Services.Interfaces;
using TimeZoneManager.Services.Model;
using TimeZoneManager.ViewModel;

namespace TimeZoneManager.Controllers
{
    [Route("api/[controller]")]
    [WebApiExceptionFilter]
    [Authorize(Policy = "Managers")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IMapper mapper, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
            _mapper = mapper;
        }

        //GET api/users
        [HttpGet]
        public UsersViewModelData Get([FromQuery]int page = 1, [FromQuery]int pageSize = 5)
        {
            _logger.LogTrace("GET api/users");
            int total;
            var viewModel = _mapper.Map<IList<UsersViewModel>>(_usersService.GetAll(new FilterQuery
            {
                PageSize = pageSize,
                Page = page
            }, out total));

            return new UsersViewModelData
            {
                Users = viewModel,
                Total = total
            };
        }

        //GET api/users/login
        [HttpGet("{loginName}")]
        public async Task<UsersViewModel> Get(string loginName)
        {
            _logger.LogTrace("GET api/users/login");
            return _mapper.Map<UsersViewModel>(await _usersService.FindByName(loginName));
        }

        //POST api/users
        [HttpPost]
        [ValidateModel]
        public async Task<object> Create([FromBody]NewUserViewModel registerViewModel)
        {
            try
            {
                _logger.LogTrace("POST api/users");

                var model = _mapper.Map<Register>(registerViewModel);
                var result = await _usersService.RegisterNewUser(model, registerViewModel.Role);

                return result;
            }
            catch (UserExistsException)
            {
                return new { UserExists = true };
            }
        }

        // PUT api/users/{name}
        [HttpPut("{name}")]
        [ValidateModel]
        public async Task<IdentityResult> Put(string name, [FromBody]UsersViewModel usersViewModel)
        {
            _logger.LogTrace("PUT api/users/{name}");
            var model = _mapper.Map<Register>(usersViewModel);
            return await _usersService.UpdateUser(model, usersViewModel.Role);
        }

        //DELETE api/users/{name}
        [HttpDelete("{name}")]
        public async Task<IdentityResult> Delete(string name)
        {
            if (name.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Unable to delete admin user");
            }
            _logger.LogTrace("DELETE api/users/{name}");
            return await _usersService.DeleteUser(name);
        }
    }
}
