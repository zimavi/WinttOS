using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using WinttOS.Core;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Benchmark;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.Misc
{
    internal class PlayBadAppleCommand : Command
    {
        public PlayBadAppleCommand(string[] commandValues) : base(commandValues)
        {
            Description = "Plays bad apple. Requires quite a bit of RAM!";
        }

        public override ReturnInfo Execute()
        {
            if (!WinttOS.IsTty)
                return new(this, ReturnCode.ERROR, "Can only play in TTY!");

            string source = "";
            bool found = false;

            SystemIO.STDOUT.PutLine("Searching for frames...");
            foreach(var disk in GlobalData.FileSystem.Disks)
            {
                foreach(var part in disk.Partitions)
                {
                    if(File.Exists(part.RootPath + @"\bad_apple\allFrames.txt"))
                    {
                        found = true;
                        source = part.RootPath + @"\bad_apple\allFrames.txt";
                    }
                }
            }

            SystemIO.STDOUT.PutLine("Loading Frames. This may take a while...");

            var time = new Stopwatch();
            time.Start();

            var handle = File.ReadAllText(source);

            var frames = handle.Split("SPLIT");

            SystemIO.STDOUT.PutLine("Done. Took " + time.TimeElapsed.ToString() +
                "\nPress any key to play...");

            Console.ReadKey();

            //DateTime nextFrame = DateTime.Now;

            FullScreenCanvas.Disable();
            WinttOS.Tty = null;
            WinttOS.Tty = new(1280, 1024);

            int targetFps = 1000 / 30;

            var watchdog = new Stopwatch();

            for (int i = 0; i < frames.Length; i++)
            {

                watchdog.Start();

                WinttOS.Tty.X = 0;
                WinttOS.Tty.Y = 0;
                WinttOS.Tty.WriteLine(frames[i]);

                int remaining = targetFps - (int)watchdog.TimeElapsed.TotalMilliseconds;

                if (remaining > 0)
                {
                    Thread.Sleep(remaining);
                }
            }

            SystemIO.STDOUT.PutLine("Done.");
            Console.ReadKey();

            FullScreenCanvas.Disable();
            WinttOS.Tty = null;
            WinttOS.Tty = new(1920, 1080);

            return new(this, ReturnCode.OK);
        }
    }
}
