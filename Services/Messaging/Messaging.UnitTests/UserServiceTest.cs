using Messaging.API.Services;
using Messaging.Core.Entities.UserAggregate;
using Messaging.Core.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Messaging.UnitTests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        private UserService CreateService()
        {
            return new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task IsSenderBlockedByReceiver_success()
        {
            var expectedResult = true;
            //Act
            _userRepositoryMock.Setup(w => w.AnyAsync(It.IsAny<ISpecification<User>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(expectedResult));

            //Arrange
            var service = CreateService();
            var result = await service.IsSenderBlockedByReceiver(default(Guid), default(Guid));

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task BlockUser_success()
        {
            //Act
            var expectedResult = true;

            _userRepositoryMock.Setup(w => w.AnyAsync(It.IsAny<ISpecification<User>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(default(bool)));
            _userRepositoryMock.Setup(w => w.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(default(User)));
            _userRepositoryMock.Setup(w => w.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(default(User)));
            _userRepositoryMock.Setup(w => w.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(expectedResult));

            //Arrange
            var service = CreateService();
            var result = await service.BlockUser(Guid.NewGuid(), Guid.NewGuid());

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
