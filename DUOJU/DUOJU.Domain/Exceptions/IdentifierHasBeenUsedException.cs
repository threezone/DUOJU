using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public class IdentifierHasBeenUsedException : BasicSystemException
    {
        public IdentifierHasBeenUsedException() : base() { }

        public IdentifierHasBeenUsedException(string message) : base(message) { }

        public IdentifierHasBeenUsedException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public IdentifierHasBeenUsedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
