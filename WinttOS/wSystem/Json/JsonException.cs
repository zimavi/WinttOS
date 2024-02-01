using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.wSystem.Json
{
    public class JsonException : Exception
    {
        public JsonException()
        {
        }

        public JsonException(string message) : base(message)
        {
        }

        public JsonException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JsonException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
