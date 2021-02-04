using Messaging.API.ApiModels;
using Messaging.API.Controllers;
using Messaging.API.Services;
using Messaging.Core.Entities.MessageAggregate;
using Messaging.Core.Exceptions;
using Messaging.Core.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Messaging.UnitTests
{
    public class MessagingServiceTest
    {
        private readonly Mock<IMessageRepository> _repositoryMock;
        private readonly Mock<IUserService> _userServiceMock;

        public MessagingServiceTest()
        {
            _repositoryMock = new Mock<IMessageRepository>();
            _userServiceMock = new Mock<IUserService>();
        }

        private MessagingService CreateService()
        {
            return new MessagingService(_repositoryMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public async Task GetMessages_returns_same_data()
        {
            //Act
            var fakeUserId = Guid.NewGuid();
            var fakePageIndex = 0;
            var fakePageSize = 10;
            var fakeTotalItems = 1;
            var fakeMessageApiList = new List<MessageApiModel>();

            var fakeMessageApiModel = new PaginatedItemsApiModel<MessageApiModel>(fakePageIndex, fakePageSize, fakeTotalItems, fakeMessageApiList);

            _repositoryMock.Setup(w => w.ListAsync(It.IsAny<ISpecification<Message>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(default(List<Message>)));
            _repositoryMock.Setup(w => w.CountAsync(It.IsAny<ISpecification<Message>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(fakeTotalItems));

            //Arrenge
            var service = CreateService();
            var result = await service.GetMessages(fakeUserId, fakePageIndex, fakePageSize);

            //Assert
            Assert.Equal(result.Data, fakeMessageApiModel.Data);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task SendMessage_success(bool data, bool commitResult)
        {
            //Act
            var fakeSenderUserId = Guid.NewGuid();
            var fakeRecevierId = Guid.NewGuid();
            var fakeMessageText = "hello";
            var generatedMessageId = 1;
            var fakeModel = new SendMessageApiModel(fakeSenderUserId, fakeRecevierId, fakeMessageText);
            _userServiceMock.Setup(w => w.IsSenderBlockedByReceiver(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(false));

            _repositoryMock.Setup(w => w.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(default(Message)));
            _repositoryMock.Setup(w => w.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(commitResult));

            //Arrenge
            var service = CreateService();
            var result = await service.SendMessage(fakeModel);

            //Assert
            Assert.Equal(data, result);
        }

        [Fact]
        public async Task SendMessage_throws_MessagingDomainException_when_user_blocked()
        {
            //Act
            var fakeSenderUserId = Guid.NewGuid();
            var fakeRecevierId = Guid.NewGuid();
            var fakeMessageText = "hello";
            var fakeModel = new SendMessageApiModel(fakeSenderUserId, fakeRecevierId, fakeMessageText);

            _userServiceMock.Setup(w => w.IsSenderBlockedByReceiver(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));

            //Arrenge
            var service = CreateService();
            async Task act() => await service.SendMessage(fakeModel);

            //Assert
            await Assert.ThrowsAsync<MessagingDomainException>(act);
        }
    }
}
