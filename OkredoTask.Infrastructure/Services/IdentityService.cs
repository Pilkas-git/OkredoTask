using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OkredoTask.Core.Constants;
using OkredoTask.Infrastructure.Models;
using OkredoTask.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public IdentityService(UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            IUserService userService,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _userService = userService;
            _userRepository = userRepository;
        }

        public async Task<RegistrationResponse> RegisterAsync(string email, string firstName, string lastName, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new RegistrationResponse() { Errors = new List<string>() { "User already exists" } };
            }

            var newUser = new IdentityUser() { Email = email, UserName = email };
            var isCreated = await _userManager.CreateAsync(newUser, password);
            if (isCreated.Succeeded)
            {
                var userId = await _userService.AddUserAsync(firstName, lastName, email);

                var jwtToken = GenerateJwtTokenAsync(newUser, userId.ToString());

                return new RegistrationResponse() { Token = await jwtToken };
            }

            return new RegistrationResponse() { Errors = isCreated.Errors.Select(x => x.Description).ToList() };
        }

        public async Task<(string token, string error)> LoginAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser == null)
            {
                return (string.Empty, "User not found");
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, password);

            if (isCorrect)
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                var jwtToken = await GenerateJwtTokenAsync(existingUser, user.UserId.ToString());

                return (jwtToken, string.Empty);
            }

            return (string.Empty, "Invalid authentication error");
        }

        private async Task<string> GenerateJwtTokenAsync(IdentityUser user, string userId)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimsConstants.Id, user.Id),
                new Claim(ClaimsConstants.UserId, userId),
                new Claim(ClaimTypes.Name, user.Email), //Used by nlog to track user
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            if (await _userManager.IsInRoleAsync(user, RoleConstants.Admin))
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, RoleConstants.Admin));
            }

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}