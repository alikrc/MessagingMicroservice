using System;
using System.Threading.Tasks;
using WebBffAggregator.ApiModels;
using WebBffAggregator.InternalApiModels;

namespace WebBffAggregator.Services
{
    public interface IMessagingService
    {
        Task<PaginatedItemsApiModel<GetMyMessagesApiModel>> GetMessages(Guid userId, int pageIndex, int pageSize);
        Task BlockUser(Guid userIdtoBlock);
        Task SendMessage(SendMessageInternalApiModel model);
    }
}