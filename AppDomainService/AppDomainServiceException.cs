using System;
using System.Runtime.Serialization;

namespace AppDomainService
{
    [Serializable]
    public class AppDomainServiceException : Exception
    {
        public AppDomainServiceException(string message)
            : base(message) { }

        public AppDomainServiceException(string message, Exception innerException) 
            : base(message, innerException) { }

        protected AppDomainServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}