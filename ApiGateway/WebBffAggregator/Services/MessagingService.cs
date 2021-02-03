using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebBffAggregator.ApiModels;
using WebBffAggregator.Config;
using WebBffAggregator.InternalApiModels;

namespace WebBffAggregator.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly string _remoteServiceBaseUrl = $"{Environment.GetEnvironmentVariable("MessagingApiUrl")}/api/v1/Message/";
        private readonly HttpClient _httpClient;
        private readonly IIdentityService _identityService;

        public MessagingService(HttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task<PaginatedItemsApiModel<GetMyMessagesApiModel>> GetMessages(Guid userId, int pageIndex, int pageSize)
        {
            var uri = UrlsConfig.Messaging.GetMessages(_remoteServiceBaseUrl, userId, pageSize, pageIndex);

            var responseString = await _httpClient.GetStringAsync(uri);

            //not worked in global settings
            //var option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            //var internalResponse = JsonSerializer.Deserialize<PaginatedItemsApiModel<MessageInternalApiModel>>(responseString, option);

            //changed to newtonsoft
            var internalResponse = JsonConvert.DeserializeObject<PaginatedItemsApiModel<MessageInternalApiModel>>(responseString);

            var response = new PaginatedItemsApiModel<GetMyMessagesApiModel>(internalResponse.PageIndex,
                internalResponse.PageSize, internalResponse.Count, new List<GetMyMessagesApiModel>());

            foreach (var item in internalResponse.Data)
            {
                response.Data.Add(new GetMyMessagesApiModel(item));
            }

            return response;
        }

        public async Task BlockUser(Guid userIdtoBlock)
        {
            var uri = UrlsConfig.Messaging.BlockUser(_remoteServiceBaseUrl, userIdtoBlock);

            var response = await _httpClient.PostAsync(uri, null);

            response.EnsureSuccessStatusCode();

        }

        public async Task SendMessage(SendMessageInternalApiModel model)
        {
            var uri = UrlsConfig.Messaging.SendMessage(_remoteServiceBaseUrl);

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();
        }

    }
}