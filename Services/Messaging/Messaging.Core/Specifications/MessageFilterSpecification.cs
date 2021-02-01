using Messaging.Core.Entities.MessageAggregate;
using System;
using System.Linq.Expressions;

namespace Messaging.Core.Specifications
{
    public class MessageFilterSpecification : BaseSpecification<Message>
    {
        public MessageFilterSpecification(Expression<Func<Message, bool>> criteria = null)
            : base(criteria)
        {
        }
    }
}
