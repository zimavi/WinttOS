namespace WinttOS.Core.Utils.Kernel
{
    public sealed class HALException
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
