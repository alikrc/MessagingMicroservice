using System;
using System.Threading.Tasks;

namespace WebBffAggregator.Services
{
    public interface IIdentityService
    {
        Guid GetUserId();
        string GetUserName();
        Task<Guid?> GetUserIdByUsername(string username);
    }
}