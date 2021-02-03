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
        public async Task Get_messages_success()
        {
            //Act
            var fakeUserId = Guid.NewGuid();
            var fakePageIndex = 0;
            var fakePageSize = 10;
            var fakeCount = 1;

            var fakeMessageApiModel = new PaginatedItemsApiModel<MessageApiModel>(fakePageIndex, fakePageSize, fakeCount, new List<MessageApiModel>());

            _messagingServiceMock.Setup(w => w.GetMyMessages(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(fakeMessageApiModel));

            //Arrenge
            var controller = CreateDefaultController();
            var actionResult = await controller.GetMessages(fakeUserId) as OkObjectResult;

            //Assert
            Assert.Equal(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal((actionResult.Value as PaginatedItemsApiModel<MessageApiModel>), fakeMessageApiModel);
        }

    }
}
