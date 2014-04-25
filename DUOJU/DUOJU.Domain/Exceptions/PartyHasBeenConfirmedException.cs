using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public class PartyHasBeenConfirmedException : BasicSystemException
    {
        public PartyHasBeenConfirmedException() : base() { }

        public PartyHasBeenConfirmedException(string message) : base(message) { }

        public PartyHasBeenConfirmedException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public PartyHasBeenConfirmedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
