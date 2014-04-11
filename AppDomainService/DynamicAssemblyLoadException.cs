using System;
using System.Runtime.Serialization;

namespace AppDomainService
{
    [Serializable]
    public sealed class DynamicAssemblyLoadException : Exception
    {
        public DynamicAssemblyLoadException(string message)
            : base(message)
        {
        }

        public DynamicAssemblyLoadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private DynamicAssemblyLoadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
