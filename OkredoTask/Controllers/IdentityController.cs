using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkredoTask.Infrastructure.Services.Interfaces;
using OkredoTask.Web.Api;
using OkredoTask.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkredoTask.Web.Controllers
{
    public class IdentityController : BaseApiController
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly IIdentityService _identityService;

        public IdentityController(
            ILogger<IdentityController> logger,
            IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationModel userRegistrationModel)
        {
            try
            {
                var response = await _identityService.RegisterAsync(userRegistrationModel.Email,
                                    userRegistrationModel.FirstName, userRegistrationModel.LastName, userRegistrationModel.Password);

                return string.IsNullOrEmpty(response.Token)
                    ? BadRequest(new AuthResultViewModel() { Errors = response.Errors })
                    : Ok(new AuthResultViewModel() { Token = response.Token });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Identity Controller when registering user");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginModel userLoginModel)
        {
            try
            {
                var (token, error) = await _identityService.LoginAsync(userLoginModel.Email, userLoginModel.Password);

                return string.IsNullOrEmpty(token)
                    ? BadRequest(new AuthResultViewModel() { Errors = new List<string>() { error } })
                    : Ok(new AuthResultViewModel() { Token = token });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Occured in Identity Controller login");
                return BadRequest();
            }
        }
    }
}