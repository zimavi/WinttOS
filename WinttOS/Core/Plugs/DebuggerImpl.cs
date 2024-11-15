using Cosmos.Debug.Kernel;
using IL2CPU.API.Attribs;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.Core.Plugs
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
