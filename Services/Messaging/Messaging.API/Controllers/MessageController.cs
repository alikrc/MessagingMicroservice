using Messaging.API.ApiModels;
using Messaging.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Messaging.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMessagingService _messagingService;
        private readonly IUserService _userService;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IIdentityService identityService, IMessagingService messagingService, ILogger<MessageController> logger, IUserService userService)
        {
            _identityService = identityService;
            _messagingService = messagingService;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [Route(nameof(GetMyMessages))]
        [ProducesResponseType(typeof(PaginatedItemsApiModel<MessageApiModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<MessageApiModel>> GetMyMessages([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var userId = _identityService.GetUserId();

            var messages = await _messagingService.GetMyMessages(userId, pageIndex, pageSize);

            return Ok(messages);
        }

        [HttpPost]
        [Route(nameof(BlockUser))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> BlockUser([FromQuery] Guid userIdtoBlock)
        {
            await _userService.BlockUser(_identityService.GetUserId(), userIdtoBlock);

            return Ok();
        }

        [HttpPost]
        [Route(nameof(SendMessage))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SendMessage([FromBody] CreateMessageApiModel createMessageApiModel)
        {
            //TODO
            //var receiverId =messageApiModel.rece

            var receiverId = Guid.Empty;
            var id = await _messagingService.CreateMessage(_identityService.GetUserId(), receiverId, createMessageApiModel.MessageText);

            return CreatedAtAction(nameof(GetMyMessages), new { id = id }, null);
        }
    }
}
