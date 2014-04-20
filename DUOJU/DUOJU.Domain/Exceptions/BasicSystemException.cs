using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public abstract class BasicSystemException : Exception
    {
        public BasicSystemException() : base() { }

        public BasicSystemException(string message) : base(message) { }

        public BasicSystemException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public BasicSystemException(string message, Exception innerException) : base(message, innerException) { }
    }
}
