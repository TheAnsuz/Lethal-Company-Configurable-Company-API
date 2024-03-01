using System;
using System.Runtime.Serialization;

namespace Amrv.ConfigurableCompany.Core
{
    public class BuildingException : Exception
    {
        public BuildingException() { }

        public BuildingException(string message) : base(message) { }

        public BuildingException(string message, Exception innerException) : base(message, innerException) { }

        protected BuildingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
