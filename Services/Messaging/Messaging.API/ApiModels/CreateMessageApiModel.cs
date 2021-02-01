using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messaging.API.ApiModels
{
    public class CreateMessageApiModel
    {
        public Guid ReceiverUserName { get; set; }
        public string MessageText { get; set; }
    }
}
