using Messaging.Core.Entities.MessageAggregate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Messaging.Core.Specifications
{
    public class MessageFilterPaginatedSpecification : BaseSpecification<Message>
    {
        public MessageFilterPaginatedSpecification(int skip, int take, Expression<Func<Message, bool>> criteria = null)
            : base(criteria)
        {
            ApplyPaging(skip, take);
        }
    }
}
