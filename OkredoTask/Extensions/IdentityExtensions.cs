using OkredoTask.Core.Constants;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace OkredoTask.Web.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetEmail(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.Email);

            return claim?.Value ?? string.Empty;
        }

        public static Guid GetUserId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(ClaimsConstants.UserId);

            return Guid.Parse(claim?.Value);
        }
    }
}