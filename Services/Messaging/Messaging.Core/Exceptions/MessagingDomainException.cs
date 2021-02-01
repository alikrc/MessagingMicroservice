using System;

namespace Messaging.Core.Exceptions
{
    public class MessagingDomainException : Exception
    {
        public MessagingDomainException()
        {
        }

        public MessagingDomainException(string message) : base(message)
        {
        }

        public MessagingDomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
