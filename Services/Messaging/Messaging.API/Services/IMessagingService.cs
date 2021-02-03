using Messaging.API.ApiModels;
using System;
using System.Threading.Tasks;

namespace Messaging.API.Services
{
    public interface IMessagingService
    {
        Task<PaginatedItemsApiModel<MessageApiModel>> GetMyMessages(Guid userId, int pageIndex, int pageSize);
        Task<int> SendMessage(SendMessageApiModel createMessageApiModel);
    }
}