using IL2CPU.API.Attribs;
using static Cosmos.Core.INTs;

namespace WinttOS.Core.Plugs
{
    [Plug(Target = typeof(Cosmos.Core.INTs))]
    public class INTs
    {
        public static void HandleException(uint aEIP, string aDescription, string aName, ref IRQContext ctx, uint lastKnownAddressValue = 0)
        {
            const string xHex = "0123456789ABCDEF";

            string ctxInterrupt = "";
            ctxInterrupt = ctxInterrupt + xHex[(int)((ctx.Interrupt >> 4) & 0xF)];
            ctxInterrupt = ctxInterrupt + xHex[(int)(ctx.Interrupt & 0xF)];

            string lastKnownAddress = "";

            if (lastKnownAddressValue != 0)
            {
                lastKnownAddress = lastKnownAddress + xHex[(int)((lastKnownAddressValue >> 28) & 0xF)];
                lastKnownAddress = lastKnownAddress + xHex[(int)((lastKnownAddressValue >> 24) & 0xF)];
                lastKnownAddress = lastKnownAddress + xHex[(int)((lastKnownAddressValue >> 20) & 0xF)];
                lastKnownAddress = lastKnownAddress + xHex[(int)((lastKnownAddressValue >> 16) & 0xF)];
                lastKnownAddress = lastKnownAddress + xHex[(int)((lastKnownAddressValue >> 12) & 0xF)];
                lastKnownAddress = lastKnownAddress + xHex[(int)((lastKnownAddressValue >> 8) & 0xF)];
                lastKnownAddress = lastKnownAddress + xHex[(int)((lastKnownAddressValue >> 4) & 0xF)];
                lastKnownAddress = lastKnownAddress + xHex[(int)(lastKnownAddressValue & 0xF)];
            }

            Kernel.WinttRaiseHardError("Unknown excpetion accured!", new Utils.Kernel.HALException(aName, aDescription, lastKnownAddress, ctxInterrupt));
        }
    }
}
