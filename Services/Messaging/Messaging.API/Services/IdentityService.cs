using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;

namespace Messaging.API.Services
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Guid GetUserId()
        {
            var idClaim = _context.HttpContext.User.FindFirst(JwtClaimTypes.Subject);

            return !String.IsNullOrEmpty(idClaim.Value) ? Guid.Parse(idClaim.Value) : throw new ArgumentNullException("User id is null or empty");
        }

        public string GetUserName()
        {
            var userNameClaim = _context.HttpContext.User.FindFirst(JwtClaimTypes.PreferredUserName);

            return !String.IsNullOrEmpty(userNameClaim.Value) ? userNameClaim.Value : throw new ArgumentNullException("Username is null or empty");
        }
    }
}
