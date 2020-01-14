using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Xpandion.WebSite.Services
{
    [Serializable]
    public class CsvProcessorException : ApplicationException
    {
        public CsvProcessorException()
        { }
        public CsvProcessorException(string message)
            : base(message)
        { }
        public CsvProcessorException(string message, Exception inner)
            : base(message, inner)
        { }
        protected CsvProcessorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
