using Messaging.Core.Entities.UserAggregate;
using Messaging.Core.Interfaces;

namespace Messaging.Infrastructure.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MessagingDbContext dbContext) : base(dbContext)
        {
            //public void FindBlockedUser()
            //{
                

            //}
        }

    }
}
