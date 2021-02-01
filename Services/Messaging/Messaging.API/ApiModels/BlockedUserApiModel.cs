using System;

namespace Messaging.API.ApiModels
{
    public class BlockedUserApiModel
    {
        public Guid BlockingUserId { get; set; }
        public Guid BlockedUserId { get; set; }
    }
}
