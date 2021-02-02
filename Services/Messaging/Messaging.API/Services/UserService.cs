﻿using Messaging.Core.Entities.UserAggregate;
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
            var blockingUser = await _userRepository.GetByIdAsync(blockingUserId);
            if (blockingUser == null)
            {
                blockingUser = new User(blockingUserId);

                await _userRepository.AddAsync(blockingUser);

                await _userRepository.UnitOfWork.CommitAsync();
            }

            var isuserToBeBlockedExists = await _userRepository.AnyAsync(w => w.Id == userIdtoBlock);
            if (isuserToBeBlockedExists == false)
            {
                var userToBeBlocked = new User(userIdtoBlock);

                await _userRepository.AddAsync(userToBeBlocked);

                await _userRepository.UnitOfWork.CommitAsync();
            }

            blockingUser.UsersBlockedByUser.Add(blockedPeople);

            await _userRepository.UpdateAsync(blockingUser);

            return await _userRepository.UnitOfWork.CommitAsync();
        }
    }
}
