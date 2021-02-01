using System;
using System.Threading.Tasks;
using WebBffAggregator.Models;

namespace WebBffAggregator.Services
{
    public interface IMessagingService
    {
        Task<int> CreateMessage(Guid senderId, Guid receiverId, string messageText);
        Task<PaginatedItemsApiModel<MessageApiModel>> GetMyMessages(Guid userId, int pageIndex, int pageSize);
        Task BlockUser(Guid userIdtoBlock);
    }
}