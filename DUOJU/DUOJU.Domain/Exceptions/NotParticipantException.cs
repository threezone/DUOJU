using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public class NotParticipantException : BasicSystemException
    {
        public NotParticipantException() : base() { }

        public NotParticipantException(string message) : base(message) { }

        public NotParticipantException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public NotParticipantException(string message, Exception innerException) : base(message, innerException) { }
    }
}
