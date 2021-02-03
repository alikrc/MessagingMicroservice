using System;
using WebBffAggregator.CommonModels;

namespace WebBffAggregator.InternalApiModels
{
    public class SendMessageInternalApiModel : BaseSendMessageApiModel
    {
        // it may be set to logged in user but since messaging service is internal usage only
        // i made generic for allowing senderId to be set
        public Guid SenderUserId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public SendMessageInternalApiModel(Guid senderUserId, Guid receiverUserId, string messageText)
        {
            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            MessageText = messageText;
        }
    }
}
