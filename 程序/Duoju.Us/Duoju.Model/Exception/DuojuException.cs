using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duoju.Model.Exception
{
    [Serializable]
    public class DuoJuException: ApplicationException
    {
        public DuoJuException(string message)
            : base(message)
        { 
        }

        public DuoJuException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}
