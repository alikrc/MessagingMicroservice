using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using WebBffAggregator.ApiModels;
using WebBffAggregator.InternalApiModels;
using WebBffAggregator.Services;

namespace WebBffAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessagingService _messagingService;
        private readonly IIdentityService _identityService;

        public MessageController(ILogger<MessageController> logger, IMessagingService messagingService, IIdentityService identityService)
        {
            _logger = logger;
            _messagingService = messagingService;
            _identityService = identityService;
        }

        [HttpGet]
        [Route(nameof(GetMyMessages))]
        [ProducesResponseType(typeof(PaginatedItemsApiModel<GetMyMessagesApiModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<MessageInternalApiModel>> GetMyMessages([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            pageSize = pageSize <= 0 ? 10 : pageSize;

            pageSize = pageSize > 250 ? 250 : pageSize;

            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            
            var loggedUserId = _identityService.GetCurrentUserId();

            var messages = await _messagingService.GetMessages(loggedUserId, pageIndex, pageSize);

            return Ok(messages);
        }

        [HttpPost]
        [Route(nameof(BlockUser))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> BlockUser([FromQuery] string userNameToBlock)
        {
            if (string.IsNullOrEmpty(userNameToBlock))
            {
                throw new ArgumentNullException("Username is null or empty");
            }
            var userIdtoBlock = await _identityService.GetUserIdByUsername(userNameToBlock);
            if (userIdtoBlock == null)
            {
                return NotFound("User not found");
            }

            await _messagingService.BlockUser(userIdtoBlock);

            return Ok();
        }

        [HttpPost]
        [Route(nameof(SendMessage))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SendMessage(SendMessageApiModel apiModel)
        {
            if (string.IsNullOrEmpty(apiModel.UsernameToSend))
            {
                return BadRequest("UsernameToSend is null or empty");
            }
            if (string.IsNullOrEmpty(apiModel.MessageText))
            {
                return BadRequest("MessageText is null or empty");
            }
            var userIdtoSend = await _identityService.GetUserIdByUsername(apiModel.UsernameToSend);
            if (userIdtoSend == Guid.Empty)
            {
                return NotFound("User not found");
            }

            var sendMessageModel = new SendMessageInternalApiModel(_identityService.GetCurrentUserId(), userIdtoSend, apiModel.MessageText);

            await _messagingService.SendMessage(sendMessageModel);

            return Ok();
        }

    }
}
