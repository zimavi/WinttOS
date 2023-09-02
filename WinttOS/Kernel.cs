using System;
using System.Collections.Generic;
using System.Text;
using WinttOS.Base;
using Sys = Cosmos.System;
using System.Linq;
using WinttOS.Base.Utils.Commands;
using WinttOS.Base.Programs.RunCommands;

namespace WinttOS
{
    public class Kernel : Sys.Kernel
    {
        

        protected override void BeforeRun()
        {
            GlobalData.fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(GlobalData.fs);
            GlobalData.fs.Initialize(true);

            manager = new CommandManager();

            foreach(Cosmos.HAL.NetworkDevice device in Cosmos.HAL.NetworkDevice.Devices)
                device?.Enable();

            Console.Clear();

            Console.WriteLine("WinttOS loaded successfully!");


            manager.registerCommand(new mivCommand("miv"));
            manager.registerCommand(new CatUtilCommand("cat"));
        }

        private CommandManager manager;

        protected override void Run()
        {
            Console.Write(@$"0\{GlobalData.currDir}> ");
            string input = Console.ReadLine();
            string[] split = input.Split(' ');

            string response = this.manager.processInputExample(input);
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
