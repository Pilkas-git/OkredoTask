using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkredoTask.Core.Constants;
using OkredoTask.Infrastructure;
using OkredoTask.Web.Api;
using OkredoTask.Web.Extensions;
using OkredoTask.Web.Models;
using System;
using System.Threading.Tasks;

namespace OkredoTask.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(
            ILogger<UserController> logger,
            IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets current user data
        /// </summary>
        /// <returns></returns>
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(User.Identity.GetUserId());

                return Ok(UserViewModel.ToModel(user));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in user Controller, get user");
                return BadRequest();
            }
        }
    }
}