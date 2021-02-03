using Messaging.Core.Exceptions;
using Messaging.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Messaging.Core.Entities.UserAggregate
{
    public class User : BaseEntity, IAggregateRoot
    {
        public Guid Id { get; private set; }

        // i didn't use this prop but it can be used for caching purposes 
        public string UserName { get; private set; }

        /// <summary>
        /// Navigation prop for people blocked by user.
        /// </summary>
        public List<BlockedPeople> UsersBlockedByUser { get; private set; } = new List<BlockedPeople>();

        /// <summary>
        /// Navigation prop for people blocks user
        /// </summary>
        public List<BlockedPeople> UsersBlockUser { get; private set; } = new List<BlockedPeople>();

        public User(Guid id)
        {
            Id = Guid.Empty != id ? id : throw new MessagingDomainException(nameof(id));
        }

        public User(Guid id, string userName)
        {
            Id = Guid.Empty != id ? id : throw new MessagingDomainException(nameof(id));
            UserName = !string.IsNullOrWhiteSpace(userName) ? userName : throw new MessagingDomainException(nameof(userName));
        }
    }
}
