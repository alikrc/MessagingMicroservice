using Messaging.Core.Entities.MessageAggregate;
using Messaging.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.Infrastructure.Data.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(MessagingDbContext dbContext) : base(dbContext)
        {
        }
    }
}
