using Messaging.API.ApiModels;
using System;
using System.Threading.Tasks;

namespace Messaging.API.Services
{
    public interface IMessagingService
    {
        Task<PaginatedItemsApiModel<MessageApiModel>> GetMessages(Guid userId, int pageIndex, int pageSize);
        Task<bool> SendMessage(SendMessageApiModel createMessageApiModel);
    }
}