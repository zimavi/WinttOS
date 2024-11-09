using System;
using System.Collections.Generic;
using System.IO;
using UniLua;
using WinttOS.Core;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Registry;
using WinttOS.wSystem.Shell.bash;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.commands.Processing
{
    public sealed class RunCommand : Command
    {
        public RunCommand(string[] commandValues) : base(commandValues)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            try
            {
                string filePath = Path.Combine(GlobalData.CurrentDirectory, arguments[0]);

                string fileExtension = Path.GetExtension(filePath);

                List<string> args = new List<string>();
                if (arguments.Count > 0)
                {
                    for (int i = 1; i < arguments.Count; i++)
                    {
                        args.Add(arguments[i]);
                    }
                }

                if (fileExtension == string.Empty)
                {
                    foreach (var package in PackageManager.Packages)
                    {
                        if (package.Name == arguments[0])
                        {
                            return RunCexe(package.Executable, args);
                        }
                    }

                    return new ReturnInfo(this, ReturnCode.ERROR, "This package does not exist.");
                }
                else
                {
                    if (!File.Exists(filePath))
                    {
                        return new ReturnInfo(this, ReturnCode.ERROR, "This file does not exist.");
                    }

                    switch (fileExtension.ToLower())
                    {
                        case ".sh":
                            BashInterpreter bash = new();

                            int pid = WinttOS.ProcessManager.CurrentProcessID;
                            var list = new List<EnvKey>
                            {
                                new("USER", UsersManager.userLogged ?? "root"),
                                new("TMPDIR", @"0:\proc\" + UUID.UUIDToString(UUID.GenerateUUID()) + "\\"),
                                new("PWD", GlobalData.CurrentDirectory)
                            };
                            Registry.Environment.PerProcessEnvironment.Add(-1, list);
                            

                            WinttOS.ProcessManager.CurrentProcessID = -1;

                            bash.Parse(filePath);
                            bash.Execute();

                            Registry.Environment.PerProcessEnvironment.Remove(-1);
                            WinttOS.ProcessManager.CurrentProcessID = pid;

                            break;
                        case ".cexe":
                            byte[] executableBytes = File.ReadAllBytes(filePath);
                            Executable executable = new(executableBytes);
                            return RunCexe(executable, args);
                        case ".lua":
                            return RunLua(filePath);
                        default:
                            return new ReturnInfo(this, ReturnCode.ERROR, "Unsupported file type.");
                    }
                }

                return new ReturnInfo(this, ReturnCode.OK);
            }
            catch (Exception ex)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, ex.ToString());
            }
        }

        private ReturnInfo RunCexe(Executable executable, List<string> args)
        {
            try
            {
                ExecutableRunner runner = new();
                runner.Run(executable, args);

                return new ReturnInfo(this, ReturnCode.OK);
            }
            catch (Exception ex)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, ex.ToString());
            }
        }

        private ReturnInfo RunLua(string filePath)
        {
            try
            {
                Logger.DoOSLog("Lua -> creating new state");
                // create Lua VM instance
                var Lua = LuaAPI.NewState();

                Logger.DoOSLog("Lua -> loading libs");
                // load base libraries
                Lua.L_OpenLibs();

                Logger.DoOSLog("Lua -> storing start time");
                CommandManager.LastLuaStart = DateTime.Now;

                Logger.DoOSLog("Lua -> creating environment");
                var list = new List<EnvKey>
                            {
                                new("USER", UsersManager.userLogged ?? "root"),
                                new("TMPDIR", @"0:\proc\" + UUID.UUIDToString(UUID.GenerateUUID()) + "\\"),
                                new("PWD", GlobalData.CurrentDirectory)
                            };
                Registry.Environment.PerProcessEnvironment.Add(-1, list);

                Logger.DoOSLog("Lua -> loading lua");
                // load and run Lua script file
                var LuaScriptFile = filePath;
                var status = Lua.L_DoFile(LuaScriptFile);

                // capture errors
                if (status != ThreadStatus.LUA_OK)
                {
                    throw new Exception(Lua.ToString(-1));
                }

                // ensuare the value returned by 'main.lua' is a Lua table
                if (!Lua.IsTable(-1))
                {
                    throw new Exception("start's return value is not a table");
                }

                Logger.DoOSLog("Lua -> storing method");
                var AwakeRef = StoreMethod("main");

                Logger.DoOSLog("Lua -> popping");
                Lua.Pop(1);

                Logger.DoOSLog("Lua -> calling method");
                CallMethod(AwakeRef);

                int StoreMethod(string name)
                {
                    Logger.DoOSLog("Lua -> getting field");
                    Lua.GetField(-1, name);
                    if (!Lua.IsFunction(-1))
                    {
                        throw new Exception(string.Format("method {0} not found!", name));
                    }
                    return Lua.L_Ref(LuaDef.LUA_REGISTRYINDEX);
                }

                void CallMethod(int funcRef)
                {
                    Logger.DoOSLog("Lua -> rawgeti");
                    Lua.RawGetI(LuaDef.LUA_REGISTRYINDEX, funcRef);
                    Logger.DoOSLog("Lua -> pcall");
                    var status = Lua.PCall(0, 0, 0);
                    if (status != ThreadStatus.LUA_OK)
                    {
                        Console.WriteLine(Lua.ToString(-1));
                    }
                }
            }
            catch (Exception e)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, e.ToString());
            }

            Registry.Environment.PerProcessEnvironment.Remove(-1);

            return new ReturnInfo(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("run [file]");
        }
    }
}