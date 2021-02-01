using Messaging.Core.Exceptions;
using Messaging.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Messaging.Core.Entities.UserAggregate
{
    public class BlockedPeople
    {
        public Guid BlockingUserId { get; private set; }
        public Guid BlockedUserId { get; private set; }
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

        //TODo
        //public BlockedPeople(User blockingUser, User blockedUser)
        //{

        //    BlockingUser = blockingUser != null ? blockingUser : throw new MessagingDomainException(nameof(blockingUser));
        //    BlockedUser = blockedUser != null ? blockedUser : throw new MessagingDomainException(nameof(blockedUser));

        //    if (blockingUser.Id == blockedUser.Id)
        //    {
        //        throw new MessagingDomainException("Blocker and blocking user can't be same.");
        //    }
        //}
    }
}
