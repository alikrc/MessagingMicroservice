using System;

namespace Messaging.API.Services
{
    public interface IIdentityService
    {
        Guid GetUserId();
        string GetUserName();
    }
}