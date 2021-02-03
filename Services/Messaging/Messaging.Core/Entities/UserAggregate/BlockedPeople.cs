using Messaging.Core.Exceptions;
using System;

namespace Messaging.Core.Entities.UserAggregate
{
    /// <summary>
    /// As DDD view this is a value entity. 
    /// 
    /// I've tried to change it to owned entity but ef core has restrictions.
    /// When ef core ready this can be changed to an owned entity type.
    /// https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities
    /// </summary>
    public class BlockedPeople
    {
        public Guid BlockingUserId { get; private set; }
        public Guid BlockedUserId { get; private set; }

        // With this navigations(FKs) valid users will be added to table
        public User BlockingUser { get; set; }
        public User BlockedUser { get; set; }

        private BlockedPeople()
        {
        }
        public BlockedPeople(Guid blockingUserId, Guid blockedUserId)
        {
            if (blockingUserId == blockedUserId)
            {
                throw new MessagingDomainException("Blocker and blocking user can't be same.");
            }

            BlockingUserId = blockingUserId != Guid.Empty ? blockingUserId : throw new MessagingDomainException(nameof(blockingUserId));
            BlockedUserId = blockedUserId != Guid.Empty ? blockedUserId : throw new MessagingDomainException(nameof(blockedUserId));

        }
    }
}
