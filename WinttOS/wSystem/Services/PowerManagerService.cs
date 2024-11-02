using Cosmos.System.Graphics;
using System;
using System.Drawing;

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

        public override void OnServiceTick()
        {

            if(!isIdling)
            {
                //_timer.Stop();
                //_timer.Start();
                return;
            }

            //if (_timer.TimeElapsed.TotalMinutes >= 1)
            //    WinttOS.SystemSleep();

        }
    }
}
