using Messaging.API.ApiModels;
using Messaging.API.Controllers;
using Messaging.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Messaging.UnitTests
{
    public class MessageApiTest
    {
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<ILogger<MessageController>> _loggerMock;
        private readonly Mock<IMessagingService> _messagingServiceMock;
        private readonly Mock<IUserService> _userServiceMock;

        public MessageApiTest()
        {
            _identityServiceMock = new Mock<IIdentityService>();
            _loggerMock = new Mock<ILogger<MessageController>>();
            _messagingServiceMock = new Mock<IMessagingService>();
            _userServiceMock = new Mock<IUserService>();
        }

        private MessageController CreateDefaultController()
        {
            return new MessageController(_identityServiceMock.Object, _messagingServiceMock.Object, _loggerMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public async Task GetMessages_success()
        {
            //Act
            var fakeUserId = Guid.NewGuid();
            var fakePageIndex = 0;
            var fakePageSize = 10;
            var fakeCount = 1;

            var fakeMessageApiModel = new PaginatedItemsApiModel<MessageApiModel>(fakePageIndex, fakePageSize, fakeCount, new List<MessageApiModel>());

            _messagingServiceMock.Setup(w => w.GetMessages(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(fakeMessageApiModel));

            //Arrenge
            var controller = CreateDefaultController();
            var actionResult = await controller.GetMessages(fakeUserId) as OkObjectResult;

            //Assert
            Assert.Equal(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal((actionResult.Value as PaginatedItemsApiModel<MessageApiModel>), fakeMessageApiModel);
        }

        [Fact]
        public async Task BlockUser_success()
        {
            //Act
            var fakeUserId = Guid.NewGuid();

            _userServiceMock.Setup(w => w.BlockUser(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));

            //Arrenge
            var controller = CreateDefaultController();
            var actionResult = await controller.BlockUser(fakeUserId) as OkResult;

            //Assert
            Assert.Equal(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task SendMessage_success()
        {
            //Act
            var fakeSenderUserId = Guid.NewGuid();
            var fakeRecevierId = Guid.NewGuid();
            var fakeMessageText = "hello";
            var fakeModel = new SendMessageApiModel(fakeSenderUserId, fakeRecevierId, fakeMessageText);
            _messagingServiceMock.Setup(w => w.SendMessage(fakeModel)).Returns(Task.FromResult(true));

            //Arrenge
            var controller = CreateDefaultController();
            var actionResult = await controller.SendMessage(fakeModel) as OkResult;

            //Assert
            Assert.Equal(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
        }
    }
}