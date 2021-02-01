using Messaging.Core.Entities.MessageAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.Core.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
    }
}
