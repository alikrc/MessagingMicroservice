using System;

namespace WebBffAggregator.Models
{
    public class MessageApiModel
    {
        public int Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string MessageText { get; set; }
        public DateTimeOffset MessageDate { get; set; }
    }
}
