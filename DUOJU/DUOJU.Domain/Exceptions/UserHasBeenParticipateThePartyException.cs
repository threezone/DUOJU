using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public class UserHasBeenParticipateThePartyException : BasicSystemException
    {
        public UserHasBeenParticipateThePartyException() : base() { }

        public UserHasBeenParticipateThePartyException(string message) : base(message) { }

        public UserHasBeenParticipateThePartyException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public UserHasBeenParticipateThePartyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
