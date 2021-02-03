using System;
using WebBffAggregator.CommonModels;

namespace WebBffAggregator.InternalApiModels
{
    public class MessageInternalApiModel : BaseSendMessageApiModel
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTimeOffset MessageDate { get; set; }

        public MessageInternalApiModel()
        {

        }

        public MessageInternalApiModel(Guid senderId, Guid receiverId, DateTimeOffset messageDate, string messageText)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            MessageDate = messageDate;
            MessageText = messageText;
        }
    }
}
