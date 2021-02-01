using Messaging.Core.Entities.UserAggregate;
using Messaging.Core.Interfaces;
using Messaging.Core.Specifications;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Messaging.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> IsSenderBlockedByReceiver(Guid senderId, Guid receiverId)
        {
            Expression<Func<User, bool>> criteria = u
                => u.UsersBlockUser.Any(bu
                    => bu.BlockedUserId == senderId && bu.BlockingUserId == receiverId);

            return await _userRepository.AnyAsync(new BaseSpecification<User>(criteria));
        }

        public async Task<bool> BlockUser(Guid blockingUserId, Guid userIdtoBlock)
        {
            var blockedPeople = new BlockedPeople(blockingUserId, userIdtoBlock);
            var user = await _userRepository.GetByIdAsync(blockingUserId);
            if (user == null)
            {
                user = new User(blockingUserId);

                user.UsersBlockedByUser.Add(blockedPeople);

                await _userRepository.AddAsync(user);

                return await _userRepository.UnitOfWork.CommitAsync();
            }

            user.UsersBlockedByUser.Add(blockedPeople);
            await _userRepository.UpdateAsync(user);
            return await _userRepository.UnitOfWork.CommitAsync();
        }
    }
}
