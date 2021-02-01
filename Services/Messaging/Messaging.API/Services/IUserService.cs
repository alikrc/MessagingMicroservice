using System;
using System.Threading.Tasks;

namespace Messaging.API.Services
{
    public interface IUserService
    {
        Task<bool> IsSenderBlockedByReceiver(Guid senderId, Guid receiverId);

        Task<bool> BlockUser(Guid blockingUserId, Guid userIdtoBlock);
    }
}
