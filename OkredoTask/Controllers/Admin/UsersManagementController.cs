using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkredoTask.Core.Constants;
using OkredoTask.Infrastructure;
using OkredoTask.Web.Api;
using OkredoTask.Web.Models;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = RoleConstants.Admin)]
    [Route("Admin/Users")]
    public class UsersManagementController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersManagementController> _logger;

        public UsersManagementController(
            ILogger<UsersManagementController> logger,
            IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets user list
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetUserListAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var users = await _userRepository.GetUsersAsync(pageNumber, pageSize);

                return Ok(UserViewModel.ToModel(users));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in user management controller, get user list");
                return BadRequest();
            }
        }
    }
}