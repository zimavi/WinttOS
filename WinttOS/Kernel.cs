using System;
using WinttOS.Base;
using Sys = Cosmos.System;
using WinttOS.Base.Programs.RunCommands;
using WinttOS.Base.commands;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.HAL;
using WinttOS.Base.Utils;
using Cosmos.System.Coroutines;
using System.Collections.Generic;

namespace WinttOS
{
    public class Kernel : Sys.Kernel
    {
        public static readonly string Version = "WinttOS v0.1.0-dev, build 476";
        protected override void BeforeRun()
        {
            ShellUtils.PrintTaskResult("Registration File System", ShellTaskResult.DOING);

            GlobalData.fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(GlobalData.fs);
            GlobalData.fs.Initialize(true);

            ShellUtils.MoveCursorUp();
            ShellUtils.PrintTaskResult("Registration File System", ShellTaskResult.OK);



            ShellUtils.PrintTaskResult("Registrating commands", ShellTaskResult.DOING);

            manager = new CommandManager();

            ShellUtils.MoveCursorUp();
            ShellUtils.PrintTaskResult("Registrating commands", ShellTaskResult.OK);



            //Encoding.RegisterProvider(CosmosEncodingProvider.Instance);



            ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.DOING);
            ShellUtils.MoveCursorUp();

            if(NetworkDevice.Devices.Count != 0)
            {
                using (var xClient = new DHCPClient())
                {
                    if (xClient.SendDiscoverPacket() != -1)
                        ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.OK);
                    else
                        ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.FAILED);

                    xClient.Close();
                }
            }
            else
                ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.FAILED);

            Console.WriteLine("System initialization complete!");

            Global.PIT.Wait(500);

            Console.Clear();

            Console.WriteLine("WinttOS loaded successfully!");

            manager.registerCommand(new mivCommand("miv"));
            manager.registerCommand(new CatUtilCommand("cat"));
            manager.registerCommand(new DevModeCommand("dev-mode"));

            //CoroutinePool.Main.OnCoroutineCycle += TestMain;
            //CoroutinePool.Main.StartPool();
        }

        public static CommandManager manager { get; private set; }

        protected override void Run()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(@$"0\{GlobalData.currDir}> ");
            Console.ForegroundColor = ConsoleColor.White;
            string input = Console.ReadLine();
            string[] split = input.Split(' ');

            Console.ForegroundColor = ConsoleColor.Gray;
            string response = manager.processInput(input);
            Console.WriteLine(response);
        }

        protected override void AfterRun()
        {
            Console.WriteLine("Is now safe to turn off your computer!");
            Sys.Power.Shutdown();
            base.AfterRun();
        }
    }
}
