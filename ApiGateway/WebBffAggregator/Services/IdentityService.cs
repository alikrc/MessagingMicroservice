using IdentityModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebBffAggregator.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly string _remoteServiceBaseUrl = $"{Environment.GetEnvironmentVariable("IdentityUrl")}/Account/GetUserIdByUserName?username=";
        private IHttpContextAccessor _context;
        private readonly HttpClient _httpClient;

        public IdentityService(IHttpContextAccessor context, HttpClient httpClient)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpClient = httpClient;
        }

        public Guid GetUserId()
        {
            var idClaim = _context.HttpContext.User.FindFirst(JwtClaimTypes.Subject);

            return !string.IsNullOrEmpty(idClaim.Value) ? Guid.Parse(idClaim.Value) : throw new ArgumentNullException("User id is null or empty");
        }

        public string GetUserName()
        {
            var userNameClaim = _context.HttpContext.User.FindFirst(JwtClaimTypes.PreferredUserName);

            return !string.IsNullOrEmpty(userNameClaim.Value) ? userNameClaim.Value : throw new ArgumentNullException("Username is null or empty");
        }

        public async Task<Guid?> GetUserIdByUsername(string username)
        {
            var responseString = await _httpClient.GetStringAsync(_remoteServiceBaseUrl + username);
            try
            {
                return new Guid(responseString);
            }
            catch (Exception e)
            {
                throw new ArgumentNullException($"{nameof(username)} not found.");
            }
        }
    }
}
