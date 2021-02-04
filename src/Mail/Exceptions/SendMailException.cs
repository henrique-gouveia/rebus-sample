using System;
using System.Runtime.Serialization;

namespace Mail.Exceptions
{
    public class SendMailException : Exception
    {
        public SendMailException()
        {
        }

        public SendMailException(string message) : base(message)
        {
        }

        public SendMailException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SendMailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
