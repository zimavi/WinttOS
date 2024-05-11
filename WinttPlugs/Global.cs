using Cosmos.HAL;
using IL2CPU.API.Attribs;
using WinttOS.Core.Utils.Debugging;

namespace WinttPlugs
{
    [Plug(Target = typeof(Cosmos.System.Global))]
    public static class Global
    {
        public static void Init(TextScreenBase textScreen, bool initScrollWheel = true, bool initPS2 = true, bool initNetwork = true, bool ideInit = true)
        {
            Logger.DoOSLog("[Info] Initializing VGA console");
            Cosmos.System.Global.Console = new Cosmos.System.Console(textScreen);

            Cosmos.HAL.Global.Init(textScreen, initScrollWheel, initPS2, initNetwork, ideInit);

            Logger.DoOSLog("[Info] Initializing network stack");
            Cosmos.System.Network.NetworkStack.Initialize();
        }
    }
}
