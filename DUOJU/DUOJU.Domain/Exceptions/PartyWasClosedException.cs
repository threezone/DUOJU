using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public class PartyWasClosedException : BasicSystemException
    {
        public PartyWasClosedException() : base() { }

        public PartyWasClosedException(string message) : base(message) { }

        public PartyWasClosedException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public PartyWasClosedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
