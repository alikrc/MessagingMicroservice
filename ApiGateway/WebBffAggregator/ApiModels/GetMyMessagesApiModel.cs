using System;
using WebBffAggregator.InternalApiModels;

namespace WebBffAggregator.ApiModels
{
    // We are forwarding same model 
    // but according to consumer instead of userIds usernames can be sent also
    public class GetMyMessagesApiModel : MessageInternalApiModel
    {
        public GetMyMessagesApiModel(MessageInternalApiModel model)
        {
            SenderId = model.SenderId;
            ReceiverId = model.ReceiverId;
            MessageDate = model.MessageDate;
            MessageText = model.MessageText;
        }
    }
}
