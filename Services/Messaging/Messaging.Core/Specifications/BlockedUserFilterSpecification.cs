using Messaging.Core.Entities.UserAggregate;
using System;
using System.Linq.Expressions;

namespace Messaging.Core.Specifications
{
    public class BlockedUserFilterSpecification : BaseSpecification<BlockedPeople>
    {
        public BlockedUserFilterSpecification(Expression<Func<BlockedPeople, bool>> criteria = null)
            : base(criteria)
        {
        }
    }
}
