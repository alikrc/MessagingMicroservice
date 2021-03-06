﻿using Messaging.API.ApiModels;
using Messaging.Core.Entities.MessageAggregate;
using Messaging.Core.Entities.UserAggregate;
using Messaging.Core.Exceptions;
using Messaging.Core.Interfaces;
using Messaging.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Messaging.API.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly IMessageRepository _repository;
        private readonly IUserService _userService;
        public MessagingService(IMessageRepository repository, IUserService userService)
        {
            _repository = repository;
            _userService = userService;
        }

        public async Task<PaginatedItemsApiModel<MessageApiModel>> GetMessages(Guid userId, int pageIndex, int pageSize)
        {
            Expression<Func<Message, bool>> criteria = w => w.ReceiverId == userId || w.SenderId == userId;

            var filterPaginatedSpecification = new MessageFilterPaginatedSpecification(pageSize * pageIndex, pageSize, criteria);
            var itemsOnPage = await _repository.ListAsync(filterPaginatedSpecification);

            var filterSpecification = new MessageFilterSpecification(criteria);
            var totalItems = await _repository.CountAsync(filterSpecification);

            var messages = itemsOnPage?.Select(w => new MessageApiModel()
            {
                Id = w.Id,
                SenderId = w.SenderId,
                ReceiverId = w.ReceiverId,
                MessageText = w.MessageText,
                MessageDate = w.MessageDate,
            }) ?? new List<MessageApiModel>();

            return new PaginatedItemsApiModel<MessageApiModel>(pageIndex, pageSize, totalItems, messages);
        }

        public async Task<bool> SendMessage(SendMessageApiModel createMessageApiModel)
        {
            var isSenderBlocked = await _userService.IsSenderBlockedByReceiver(createMessageApiModel.SenderUserId, createMessageApiModel.ReceiverUserId);
            if (isSenderBlocked)
            {
                throw new MessagingDomainException("Can't send message, sender is blocked by receiver.");
            }

            var entity = new Message(createMessageApiModel.SenderUserId, createMessageApiModel.ReceiverUserId, createMessageApiModel.MessageText);

            await _repository.AddAsync(entity);

            return await _repository.UnitOfWork.CommitAsync();
        }
    }
}
