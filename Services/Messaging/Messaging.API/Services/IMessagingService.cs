﻿using Messaging.API.ApiModels;
using System;
using System.Threading.Tasks;

namespace Messaging.API.Services
{
    public interface IMessagingService
    {
        Task<int> CreateMessage(Guid senderId, Guid receiverId, string messageText);
        Task<PaginatedItemsApiModel<MessageApiModel>> GetMyMessages(Guid userId, int pageIndex, int pageSize);
    }
}