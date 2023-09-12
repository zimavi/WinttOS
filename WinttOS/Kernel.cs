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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using WinttOS.Base.Utils.Registry;
using Cosmos.Core.Memory;
using Cosmos.System.Audio;
using Cosmos.HAL.Drivers.Audio;
using Cosmos.System.Audio.IO;
using WinttOS.Base.Users;
using WinttOS.Base.Utils.Serialization;
using System.Text;
using System.Collections.Specialized;

namespace WinttOS
{
    public class Kernel : Sys.Kernel
    {
        public static readonly string Version = "WinttOS v0.1.0-dev, build 476";
        public static StringCollection ReadonlyFiles { get; internal set; }
        public static StringCollection ReadonlyDirectories { get; internal set; }

        public static UsersManager UsersManager { get; internal set; } = new(null);

        /*
        public static AudioMixer Mixer = new();
        private static AC97 driver = AC97.Initialize(bufferSize: 4096);
        private static AudioManager audioManager = new()
        {
            Stream = Mixer,
            Output = driver
        };
        */

        protected override void BeforeRun()
        {
            try
            {
                ReadonlyFiles = new()
                {
                    @"0:\Root.txt",
                    @"0:\Kudzu.txt",
                };

                ReadonlyDirectories = new()
                {
                    @"0:\WinttOS",
                    @"0:\WinttOS\System32"
                };

                //ShellUtils.PrintTaskResult("Initialize drivers", ShellTaskResult.DOING);

                //Mixer.Streams.Add(MemoryAudioStream.FromWave(Files.RawStartupSound));
                //audioManager.Enable();

                //ShellUtils.MoveCursorUp();
                //ShellUtils.PrintTaskResult("Initialize drivers", ShellTaskResult.OK);
                

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

                Console.WriteLine("NOTE! If you have more then one network apadters, please remove all except one!\n");

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

                Console.WriteLine("WinttOS loaded successfully!\n");

                manager.registerCommand(new mivCommand("miv"));
                manager.registerCommand(new CatUtilCommand("cat"));
                manager.registerCommand(new DevModeCommand("dev-mode"));
                manager.registerCommand(new ExampleCrashCommand("crash-pls"));
                manager.registerCommand(new WgetCommand("wget"));

                ShellUtils.PrintTaskResult("Registaring registry", ShellTaskResult.DOING);


                //WinttRegistry.Registry = new();
                //WinttRegistry.Registry.Registries[0].RegistryEntries.Add(new()
                //{
                //    Key = "OsVerison",
                //    Value = Version
                //});

                //ShellUtils.MoveCursorUp();
                //ShellUtils.PrintTaskResult("Registaring registry", ShellTaskResult.OK);


                //CoroutinePool.Main.OnCoroutineCycle += TestMain;
                //CoroutinePool.Main.StartPool();


                //List<User> users = new() {
                //    User.CreateAdminAccount("root", "root"),
                //    User.CreateGuestAccount(),
                //    new User.UserBuilder().SetUserName("Test")
                //                          .SetPassword("qwerty")
                //                          .Build()
                //};

                //var serializer = new WinttUserSerializer();
                //string list = serializer.SerializeList(users);
                //File.WriteAllBytes("0:\\users.dat", Encoding.UTF8.GetBytes(list));

                //Console.ReadKey(true);

                if(!UsersManager.LoadUsersData())
                {
                    Console.WriteLine("As OS did not find user's data, please specify new password for \"root\" superuser");
                    Console.Write("Password: ");
                    string pass = Console.ReadLine();
                    UsersManager.AddUser(new User.UserBuilder().SetUserName("root")
                                                               .SetPassword(pass)
                                                               .SetAccess(User.AccessLevel.Administrator)
                                                               .Build());
                    Console.Clear();
                    Console.Write("Do you want to create antoher account? (Y/n): ");
                    bool _loop = true;
                    while(_loop)
                    {
                        string name;
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Y:
                                Console.Clear();
                                Console.Write("Please enter new user name: ");
                                name = Console.ReadLine();
                                Console.Write("Please enter new user's password: ");
                                pass = Console.ReadLine();
                                if (!string.IsNullOrEmpty(pass))
                                    UsersManager.AddUser(new User.UserBuilder().SetUserName(name)
                                                                               .SetPassword(pass)
                                                                               .Build());
                                else
                                    UsersManager.AddUser(new User.UserBuilder().SetUserName(name)
                                                                               .Build());
                                UsersManager.LoginIntoUserAccount(name, pass);
                                break;
                            case ConsoleKey.N:
                                _loop = false;
                                UsersManager.LoginIntoUserAccount("root", pass);
                                break;
                            case ConsoleKey.Enter:
                                Console.Clear();
                                Console.Write("Please enter new user name: ");
                                name = Console.ReadLine();
                                Console.Write("Please enter new user's password: ");
                                pass = Console.ReadLine();
                                if (!string.IsNullOrEmpty(pass))
                                    UsersManager.AddUser(new User.UserBuilder().SetUserName(name)
                                                                               .SetPassword(pass)
                                                                               .Build());
                                else
                                    UsersManager.AddUser(new User.UserBuilder().SetUserName(name)
                                                                               .Build());
                                UsersManager.LoginIntoUserAccount(name, pass);
                                _loop = false;
                                break;
                            default:
                                Console.Clear();
                                break;
                        }
                    }
                    Console.Clear();
                }

                WinttDebugger.Trace("Cleanned " + Heap.Collect() + " objects", this);

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
                Console.Write(@$"{UsersManager.currentUser.Name}$0:\{GlobalData.currDir}> ");
                Console.ForegroundColor = ConsoleColor.White;
                string input = Console.ReadLine();
                string[] split = input.Split(' ');

                Console.ForegroundColor = ConsoleColor.Gray;
                string response = manager.processInput(input);
                Console.WriteLine(response);
                WinttDebugger.Trace("Cleanned " + Heap.Collect() + " objects", this);
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
