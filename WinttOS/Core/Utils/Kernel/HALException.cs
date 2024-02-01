using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.Kernel
{
    public class HALException
    {
        public readonly string Exception;
        public readonly string Description;
        public readonly string LastKnownAddress;
        public readonly string CTXInterrupt;

        public HALException(string exception, string description, string lastKnownAddress, string ctxInterrupt)
        {
            Exception = exception;
            Description = description;
            LastKnownAddress = lastKnownAddress;
            CTXInterrupt = ctxInterrupt;
        }
    }
}
