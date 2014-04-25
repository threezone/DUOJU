using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public class CanNotFindIdentifierException : BasicSystemException
    {
        public CanNotFindIdentifierException() : base() { }

        public CanNotFindIdentifierException(string message) : base(message) { }

        public CanNotFindIdentifierException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public CanNotFindIdentifierException(string message, Exception innerException) : base(message, innerException) { }
    }
}
