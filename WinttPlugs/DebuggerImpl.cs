using Cosmos.Debug.Kernel;
using IL2CPU.API.Attribs;
using WinttOS.Core.Utils.Debugging;

namespace WinttPlugs
{
    [Plug(Target = typeof(Debugger))]
    public class DebuggerImpl
    {
        public static void DoSend(string aText)
        {
            Logger.DoKernelLog(aText);
        }
    }
}
