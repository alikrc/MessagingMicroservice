using System;
using System.Threading.Tasks;

namespace WebBffAggregator.Services
{
    public interface IIdentityService
    {
        Guid GetCurrentUserId();
        string GetUserName();
        Task<Guid> GetUserIdByUsername(string username);
    }
}