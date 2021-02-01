using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using WebBffAggregator.Models;
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
        public async Task<ActionResult> BlockUser([FromQuery] string userNameToBlock)
        {
            if (string.IsNullOrEmpty(userNameToBlock))
            {
                throw new ArgumentNullException();
            }
            var userIdtoBlock = await _identityService.GetUserIdByUsername(userNameToBlock);
            if (userIdtoBlock == null)
            {
                throw new ArgumentNullException();
            }

            await _messagingService.BlockUser(userIdtoBlock.Value);

            return Ok();
        }
    }
}
