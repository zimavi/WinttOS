using Cosmos.System.Graphics;
using System;
using System.Drawing;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.wSystem.Services
{
    public sealed class PowerManagerService : Service
    {
        public static bool isIdling = false;
        //private Stopwatch _timer = new();

        public PowerManagerService() : base("powerd", "power.service")
        { 
            //WinttOS.OnSystemSleep.Add(HandleSystemSleepEvent); 
        }

        public void HandleSystemSleepEvent()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Services.PowerManagerService.HandleSystemSleepEvent()",
                "void()", "PowerManagerService", 19));
            WinttDebugger.Info("Going to sleep mode!");
            Canvas blackCanvas = FullScreenCanvas.GetFullScreenCanvas();
            blackCanvas.Clear(Color.Black);

            Console.ReadKey();
            WinttDebugger.Info("Coming out from sleep mode!");
            blackCanvas.Disable();
            FullScreenCanvas.Disable();
            WinttOS.IsSleeping = false;
            isIdling = false;

            WinttCallStack.RegisterReturn();
        }

        public override void OnServiceTick()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Service.PowerManagerService.ServiceTick()",
                "void()", "PowerManagerService.cs", 37));

            if(!isIdling)
            {
                //_timer.Stop();
                //_timer.Start();
                WinttCallStack.RegisterReturn();
                return;
            }

            //if (_timer.TimeElapsed.TotalMinutes >= 1)
            //    WinttOS.SystemSleep();

            WinttCallStack.RegisterReturn();
        }
    }
}
