using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public class NoPartySInitiatorException : BasicSystemException
    {
        public NoPartySInitiatorException() : base() { }

        public NoPartySInitiatorException(string message) : base(message) { }

        public NoPartySInitiatorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public NoPartySInitiatorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
