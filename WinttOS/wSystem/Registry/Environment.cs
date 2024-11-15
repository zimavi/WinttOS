using LunarLabs.Parser;
using LunarLabs.Parser.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using UniLua;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Filesystem;

namespace WinttOS.wSystem.Registry
{
    public static class Environment
    {
        private const string ETC_PATH = "/etc/";
        private const string ENV_FILE_PATH = "/etc/environment.sysenv";

        // default environment
        private const string ENV_PATH = "/usr/bin/;/lib/";
        private const string ENV_TMP = "/tmp/";

        public static List<EnvKey> GlobalEnvironment;

        // Each PID has own Environment
        // -1 reserved for Bash, Cosmos Executable and Lua
        public static Dictionary<int, List<EnvKey>> PerProcessEnvironment;

        public static void Initialize()
        {
            ShellUtils.PrintTaskResult("Loading", ShellTaskResult.DOING, "System environment");

            GlobalEnvironment = new();
            PerProcessEnvironment = new();
            
            if (!Directory.Exists(IOMapper.MapFHSToPhysical(ETC_PATH)))
            {
                Directory.CreateDirectory(IOMapper.MapFHSToPhysical(ETC_PATH));
            }

            if (File.Exists(IOMapper.MapFHSToPhysical(ENV_FILE_PATH)))
            {
                try
                {
                    string txt = File.ReadAllText(IOMapper.MapFHSToPhysical(ENV_FILE_PATH));
                    Logger.DoOSLog(txt);
                    DataNode root = JSONReader.ReadFromString(txt);

                    foreach (var node in root)
                    {
                        GlobalEnvironment.Add(new EnvKey { Name = node.Name, Value = node.Value });
                    }

                    ShellUtils.MoveCursorUp();
                    ShellUtils.PrintTaskResult("Loading", ShellTaskResult.OK, "System environment");
                }
                catch (Exception e)
                {
                    Logger.DoOSLog(e.Message);

                    ShellUtils.MoveCursorUp();
                    ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "System environment");

                    GlobalEnvironment.Clear();
                    GlobalEnvironment.Add(new EnvKey { Name = "PATH",       Value = ENV_PATH });
                    GlobalEnvironment.Add(new EnvKey { Name = "OS_VER",     Value = WinttOS.WinttVersion });
                    GlobalEnvironment.Add(new EnvKey { Name = "OS",         Value = "WinttOS"});
                    GlobalEnvironment.Add(new EnvKey { Name = "TMPDIR",     Value = ENV_TMP });

                    SaveEnvironment();
                }
            }
            else
            {
                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.WARN, "System environment: Not found");
                GlobalEnvironment.Add(new EnvKey { Name = "PATH",       Value = ENV_PATH });
                GlobalEnvironment.Add(new EnvKey { Name = "OS_VER",     Value = WinttOS.WinttVersion });
                GlobalEnvironment.Add(new EnvKey { Name = "OS",         Value = "WinttOS"});
                GlobalEnvironment.Add(new EnvKey { Name = "TMPDIR",     Value = ENV_TMP });

                SaveEnvironment();
            }
        }

        public static void SaveEnvironment()
        {
            DataNode root = DataNode.CreateObject();

            foreach(var node in GlobalEnvironment)
            {
                root.AddField(node.Name, node.Value);
            }

            File.WriteAllText(IOMapper.MapFHSToPhysical(ENV_FILE_PATH), JSONWriter.WriteToString(root));
        }

        public static void LoadEnvironment()
        {
            GlobalEnvironment.Clear();

            try
            {
                if (File.Exists(IOMapper.MapFHSToPhysical(ENV_FILE_PATH)))
                {
                    string txt = File.ReadAllText(IOMapper.MapFHSToPhysical(ENV_FILE_PATH));
                    Logger.DoOSLog(txt);
                    DataNode root = JSONReader.ReadFromString(txt);

                    foreach (var node in root)
                    {
                        GlobalEnvironment.Add(new EnvKey { Name = node.Name, Value = node.Value });
                    }
                }
                else
                {
                    GlobalEnvironment.Add(new EnvKey { Name = "PATH",       Value = ENV_PATH });
                    GlobalEnvironment.Add(new EnvKey { Name = "OS_VER",     Value = WinttOS.WinttVersion });
                    GlobalEnvironment.Add(new EnvKey { Name = "OS",         Value = "WinttOS"});
                    GlobalEnvironment.Add(new EnvKey { Name = "TMPDIR",     Value = ENV_TMP });

                    SaveEnvironment();
                }
            }
            catch
            {
                GlobalEnvironment.Add(new EnvKey { Name = "PATH",       Value = ENV_PATH });
                GlobalEnvironment.Add(new EnvKey { Name = "OS_VER",     Value = WinttOS.WinttVersion });
                GlobalEnvironment.Add(new EnvKey { Name = "OS",         Value = "WinttOS"});
                GlobalEnvironment.Add(new EnvKey { Name = "TMPDIR",     Value = ENV_TMP });

                SaveEnvironment();
            }
        }

        public static bool HasValue(string name)
        {
            foreach(var node in GlobalEnvironment)
            {
                if (node.Name == name)
                    return true;
            }
            return false;
        }
        
        public static void SetValue(string name, object value)
        {
            for(int i = 0; i < GlobalEnvironment.Count; i++)
            {
                if (GlobalEnvironment[i].Name == name)
                {
                    EnvKey node = GlobalEnvironment[i];
                    node.Value = value.ToString();
                    GlobalEnvironment[i] = node;
                    return;
                }
            }

            GlobalEnvironment.Add(new EnvKey { Name = name, Value = value.ToString() });
        }

        public static void RemoveValue(string name)
        {
            for (int i = 0; i < GlobalEnvironment.Count; i++)
            {
                if (GlobalEnvironment[i].Name == name)
                {
                    GlobalEnvironment.RemoveAt(i);
                    return;
                }
            }
        }

        public static string GetValue(string name)
        {
            foreach (var node in GlobalEnvironment)
            {
                if (node.Name == name)
                    return node.Value;
            }
            return null;
        }

        public static bool HasProcessEnvValue(int pid, string name)
        {
            if (!PerProcessEnvironment.ContainsKey(pid))
                return false;

            foreach(var node in PerProcessEnvironment[pid])
            {
                if (node.Name == name) 
                    return true;
            }

            return false;
        }

        public static void SetProcessEnvValue(int pid, string name, object value)
        {
            if (!PerProcessEnvironment.ContainsKey(pid))
                return;

            for (int i = 0; i < PerProcessEnvironment[pid].Count; i++)
            {
                if (PerProcessEnvironment[pid][i].Name == name)
                {
                    EnvKey node = PerProcessEnvironment[pid][i];
                    node.Value = value.ToString();
                    PerProcessEnvironment[pid][i] = node;
                    return;
                }
            }

            PerProcessEnvironment[pid].Add(new EnvKey { Name = name, Value = value.ToString() });
        }

        public static string GetProcessEnvValue(int pid, string name)
        {
            if (!HasProcessEnvValue(pid, name))
                return null;

            foreach (var node in PerProcessEnvironment[pid])
            {
                if (node.Name == name)
                    return node.Name;
            }

            return null;
        }
    }
}
