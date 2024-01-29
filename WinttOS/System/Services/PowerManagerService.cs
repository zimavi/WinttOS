using Cosmos.System.Graphics;
using WinttOS.Core.Utils.Debugging;
using WinttOS.System.Benchmark;
using System.Drawing;
using System;

namespace WinttOS.System.Services
{
    public class PowerManagerService : Service
    {
        public static bool isIdling = false;
        private Timer timer = new();

        public PowerManagerService() : base("pwrmgr", "PowerManagerDaemon")
        { 
            //WinttOS.OnSystemSleep.Add(HandleSystemSleepEvent); 
        }

        public void HandleSystemSleepEvent()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.PowerManagerService.HandleSystemSleepEvent()",
                "void()", "PowerManagerService", 19));
            WinttDebugger.Info("Going to sleep mode!");
            Canvas blackCanvas = FullScreenCanvas.GetFullScreenCanvas();
            blackCanvas.Clear(Color.Black);

            _ = Console.ReadKey();
            WinttDebugger.Info("Coming out from sleep mode!");
            blackCanvas.Disable();
            FullScreenCanvas.Disable();
            WinttOS.IsSleeping = false;
            isIdling = false;

            WinttCallStack.RegisterReturn();
        }

        public override void OnServiceTick()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Service.PowerManagerService.ServiceTick()",
                "void()", "PowerManagerService.cs", 37));

            if(!isIdling)
            {
                timer.RestartTimer();
                WinttCallStack.RegisterReturn();
                return;
            }

            if (timer.GetElapsedTime().TotalMinutes >= 1)
                WinttOS.SystemSleep();

            WinttCallStack.RegisterReturn();
        }
    }
}
