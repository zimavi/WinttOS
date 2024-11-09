using System;
using System.Collections.Generic;
using UniLua;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core;
using WinttOS.wSystem.Shell;
using WinttOS.wSystem.Users;
using WinttOS.wSystem.Registry;
using System.IO;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Utils;
using WinttOS.wSystem.Shell.commands.FileSystem;

namespace WinttOS.wSystem.Processing
{
    public sealed class ExecutableRunner
    {

        public void Run(Executable executable, List<string> args)
        {
            try
            {
                LuaFile.VirtualFiles = new Dictionary<string, byte[]>();

                // create Lua VM instance
                var Lua = LuaAPI.NewState();

                // load base libraries
                Lua.L_OpenLibs();

                // args
                Lua.NewTable();
                Lua.PushValue(-1);
                Lua.SetGlobal("arg");

                Lua.PushString("main.lua");
                Lua.RawSetI(-2, 0);

                for (int i = 0; i < args.Count; i++)
                {
                    Lua.PushString(args[i]);
                    Lua.RawSetI(-2, i + 1);
                }


                foreach (var source in executable.LuaSources.Keys)
                {
                    if (source == "main.lua")
                    {
                        continue;
                    }

                    LoadLuaFile(source);
                }

                if(executable.Dependencies.Count > 0)
                {
                    List<string> loadedLibs = new();

                    var di = new DirectoryInfo(@"0:\lib");
                    var infos = di.GetFileSystemInfos();
                    foreach (var info in infos)
                    {
                        if (info.IsDirectory())
                            continue;

                        if (loadedLibs.Count >= executable.Dependencies.Count)
                            break;

                        var file = @"0:\lib\" + info.Name;

                        try
                        {
                            Executable lib = new(File.ReadAllBytes(file));

                            if(!lib.IsLibrary)
                            {
                                continue;
                            }

                            foreach(var dependency in executable.Dependencies)
                            {
                                if(!lib.Name.Equals(dependency))
                                {
                                    continue;
                                }

                                if (loadedLibs.Contains(dependency))
                                    break; // break because this lib is already loaded

                                foreach (var source in lib.LuaSources.Keys)
                                {
                                    if (source == "main.lua")
                                        continue;

                                    LoadLuaLibFile(source, lib);
                                }
                                loadedLibs.Add(lib.Name);

                                break;
                            }
                        }
                        // exception should only be thrown if file is nither executable nor lib,
                        // so just ignore and continue
                        catch
                        {

                        }
                    }

                    if(loadedLibs.Count < executable.Dependencies.Count)
                    {
                        SystemIO.STDOUT.PutLine("Missing libraries found! (expected " + executable.Dependencies.Count + " found " + loadedLibs.Count + ")");
                        Lists.CompareLists(executable.Dependencies, loadedLibs, out List<string> missingLibs, out _);
                        foreach(var lib in missingLibs)
                        {
                            SystemIO.STDOUT.PutLine(lib + " missing");
                        }    
                    }
                }

                LoadLuaFile("main.lua");

                void LoadLuaFile(string fileName)
                {
                    var status = Lua.L_LoadBytes(executable.LuaSources[fileName], fileName);

                    // capture errors
                    if (status != ThreadStatus.LUA_OK)
                    {
                        throw new Exception(Lua.ToString(-1));
                    }
                }

                void LoadLuaLibFile(string filename, Executable lib)
                {
                    Console.WriteLine("    Loaded " + filename);
                    var status = Lua.L_LoadBytes(lib.LuaSources[filename], filename);

                    if (status != ThreadStatus.LUA_OK)
                    {
                        throw new Exception(Lua.ToString(-1));
                    }
                }

                CommandManager.LastLuaStart = DateTime.Now;
                var list = new List<EnvKey>
                            {
                                new("USER", UsersManager.userLogged ?? "root"),
                                new("TMPDIR", @"0:\proc\" + UUID.UUIDToString(UUID.GenerateUUID()) + "\\"),
                                new("PWD", GlobalData.CurrentDirectory)
                            };
                Registry.Environment.PerProcessEnvironment.Add(-1, list);

                var status = Lua.PCall(0, LuaDef.LUA_MULTRET, 0);
                if (status != ThreadStatus.LUA_OK)
                {
                    throw new Exception(Lua.ToString(-1));
                }

                LuaFile.VirtualFiles.Clear();
                LuaFile.VirtualFiles = null;

                Registry.Environment.PerProcessEnvironment.Remove(-1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
