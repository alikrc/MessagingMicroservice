using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebBffAggregator.Config;
using WebBffAggregator.Models;

namespace WebBffAggregator.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly string _remoteServiceBaseUrl = $"{Environment.GetEnvironmentVariable("MessagingApiUrl")}/api/v1/Message/";
        private readonly HttpClient _httpClient;

        public MessagingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CreateMessage(Guid senderId, Guid receiverId, string messageText)
        {
            throw new NotImplementedException();
        }

        public async Task BlockUser(Guid userIdtoBlock)
        {
            var uri = UrlsConfig.Messaging.BlockUser(_remoteServiceBaseUrl, userIdtoBlock);

            await _httpClient.PostAsync(uri, null);
        }

        public async Task<PaginatedItemsApiModel<MessageApiModel>> GetMyMessages(Guid userId, int pageIndex, int pageSize)
        {
            var uri = UrlsConfig.Messaging.GetMyMessages(_remoteServiceBaseUrl, pageSize, pageIndex);

            var responseString = await _httpClient.GetStringAsync(uri);

            var response = JsonConvert.DeserializeObject<PaginatedItemsApiModel<MessageApiModel>>(responseString);

            return response;
        }
    }
}