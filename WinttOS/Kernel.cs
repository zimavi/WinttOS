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
using WinttOS.Base.Utils.Debugging;
using Cosmos.System.Graphics;
using Cosmos.System.Network.Config;

namespace WinttOS
{
    public class Kernel : Sys.Kernel
    {
        public static readonly string Version = "WinttOS v0.1.0-dev, build 476";
        public static List<string> ReadonlyFiles { get; internal set; }
        public static List<string> ReadonlyDirectories { get; internal set; }
        protected override void BeforeRun()
        {
            try
            {
                ReadonlyFiles = new()
                {
                    @"0:\Root.txt",
                    @"0:\Kudzu.txt",
                };

                ReadonlyDirectories = new();

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

                Console.WriteLine("NOTE! If you have more then one network apadters, please remove all except one!");

                ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.DOING);
                ShellUtils.MoveCursorUp();

                if (NetworkDevice.Devices.Count != 0)
                {
                    using (var xClient = new DHCPClient())
                    {
                        try
                        {
                            if (xClient.SendDiscoverPacket() != -1)
                                ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.OK);
                            else
                                ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.FAILED);

                            xClient.Close();
                        }
                        catch (Exception e)
                        {
                            WinttDebugger.Critical(e.Message, true, this);
                        }
                    }
                }
                else
                    ShellUtils.PrintTaskResult("Discovering IP address", ShellTaskResult.FAILED);
                
                //DNSConfig.Add(new(8, 8, 8, 8));

                Console.WriteLine("System initialization complete!");

                Global.PIT.Wait(500);

                Console.Clear();

                Console.WriteLine("WinttOS loaded successfully!");

                manager.registerCommand(new mivCommand("miv"));
                manager.registerCommand(new CatUtilCommand("cat"));
                manager.registerCommand(new DevModeCommand("dev-mode"));
                manager.registerCommand(new ExampleCrashCommand("crash-pls"));

                //CoroutinePool.Main.OnCoroutineCycle += TestMain;
                //CoroutinePool.Main.StartPool();
            } catch (Exception ex)
            {
                WinttDebugger.Critical(ex.Message, true, this);
            }
        }

        public static CommandManager manager { get; private set; }

        protected override void Run()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(@$"0:\{GlobalData.currDir}> ");
                Console.ForegroundColor = ConsoleColor.White;
                string input = Console.ReadLine();
                string[] split = input.Split(' ');

                Console.ForegroundColor = ConsoleColor.Gray;
                string response = manager.processInput(input);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                WinttDebugger.Error("", false);
                WinttDebugger.Error("", false);
                WinttDebugger.Error("DETECTED SYSTEM CRASH!", false);
                WinttDebugger.Error("", false);
                WinttDebugger.Error("", false);
                WinttDebugger.Critical(ex.Message, true, this);
            }
        }

        protected override void AfterRun()
        {
            Console.WriteLine("Is now safe to turn off your computer!");
            Sys.Power.Shutdown();
            base.AfterRun();
        }
    }
}
