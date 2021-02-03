using System;
using WebBffAggregator.CommonModels;

namespace WebBffAggregator.InternalApiModels
{
    public class SendMessageApiModel : BaseSendMessageApiModel
    {
        public string UsernameToSend { get; set; }
    }
}
