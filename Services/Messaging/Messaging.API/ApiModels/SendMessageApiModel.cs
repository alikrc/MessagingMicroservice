using System;

namespace Messaging.API.ApiModels
{
    public class SendMessageApiModel
    {
        // it may be set to logged in user but since messaging service is internal usage only
        // i made generic for allowing senderId to be set
        public Guid SenderUserId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public string MessageText { get; set; }
        public SendMessageApiModel(Guid senderUserId, Guid receiverUserId, string messageText)
        {
            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            MessageText = messageText;
        }
    }
}
