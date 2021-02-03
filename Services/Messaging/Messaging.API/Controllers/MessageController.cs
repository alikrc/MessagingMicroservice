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
        [Route(nameof(GetMessages))]
        [ProducesResponseType(typeof(PaginatedItemsApiModel<MessageApiModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<MessageApiModel>> GetMessages([FromQuery] Guid? userId = null, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            //Since messaging service will be internal use in production, i didn't put any permission logic here to make it generic

            userId = (userId != null && userId != Guid.Empty) ? userId : _identityService.GetUserId();
            if (userId == null || userId == Guid.Empty)
            {
                return BadRequest("UserId is null or empty");
            }

            var messages = await _messagingService.GetMyMessages(userId.Value, pageIndex, pageSize);

            return Ok(messages);
        }

        [HttpPost]
        [Route(nameof(BlockUser))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> BlockUser([FromQuery] Guid userIdtoBlock)
        {
            if (userIdtoBlock == Guid.Empty)
            {
                NotFound();
            }

            await _userService.BlockUser(_identityService.GetUserId(), userIdtoBlock);

            return Ok();
        }

        [HttpPost]
        [Route(nameof(SendMessage))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SendMessage([FromBody] SendMessageApiModel createMessageApiModel)
        {
            await _messagingService.SendMessage(createMessageApiModel);

            return Ok();
        }
    }
}
