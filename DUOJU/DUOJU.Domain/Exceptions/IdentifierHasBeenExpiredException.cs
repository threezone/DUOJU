using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public class IdentifierHasBeenExpiredException : BasicSystemException
    {
        public IdentifierHasBeenExpiredException() : base() { }

        public IdentifierHasBeenExpiredException(string message) : base(message) { }

        public IdentifierHasBeenExpiredException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public IdentifierHasBeenExpiredException(string message, Exception innerException) : base(message, innerException) { }
    }
}
