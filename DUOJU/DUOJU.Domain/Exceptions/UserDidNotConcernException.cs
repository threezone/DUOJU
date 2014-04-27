using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public class UserDidNotConcernException : BasicSystemException
    {
        public UserDidNotConcernException() : base() { }

        public UserDidNotConcernException(string message) : base(message) { }

        public UserDidNotConcernException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public UserDidNotConcernException(string message, Exception innerException) : base(message, innerException) { }
    }
}
