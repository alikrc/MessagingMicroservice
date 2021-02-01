using Messaging.Core.Exceptions;
using Messaging.Core.Interfaces;
using System;

namespace Messaging.Core.Entities.MessageAggregate
{
    public class Message : BaseEntity, IAggregateRoot
    {
        public Guid SenderId { get; private set; }
        public Guid ReceiverId { get; private set; }
        public string MessageText { get; private set; }
        public DateTimeOffset MessageDate { get; private set; } = DateTimeOffset.Now;

        private Message()
        {
        }

        public Message(Guid senderId, Guid receiverId, string messageText)
        {
            SenderId = Guid.Empty != senderId ? senderId : throw new MessagingDomainException(nameof(senderId));
            ReceiverId = Guid.Empty != receiverId ? receiverId : throw new MessagingDomainException(nameof(receiverId));
            MessageText = !string.IsNullOrWhiteSpace(messageText) ? messageText : throw new MessagingDomainException(nameof(messageText));
        }
    }
}
