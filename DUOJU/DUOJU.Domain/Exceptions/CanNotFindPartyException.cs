using System;
using System.Runtime.Serialization;

namespace DUOJU.Domain.Exceptions
{
    public class CanNotFindPartyException : BasicSystemException
    {
        public CanNotFindPartyException() : base() { }

        public CanNotFindPartyException(string message) : base(message) { }

        public CanNotFindPartyException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public CanNotFindPartyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
