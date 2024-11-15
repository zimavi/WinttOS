using LunarLabs.Parser;
using LunarLabs.Parser.JSON;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;

namespace WinttOS.wSystem.Filesystem
{
    public static class IOMapper
    {
        private static Dictionary<string, string> _driveMappings;

        public static void Initialize()
        {
            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.DOING, "DMM"); // Stands for Drive Mapping Manager
            _driveMappings = new()  // default mapping
            {
                { @"0:\", "/" }     // map first drive as root
            };

            if (!Directory.Exists(@"0:\etc"))
            {
                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.WARN, "DMM: No config found, default is used");
                return;
            }

            if (!File.Exists(@"0:\etc\drive_mappings.conf"))
            {
                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.WARN, "DMM: No config found, default is used");
                return;
            }

            DataNode root = JSONReader.ReadFromString(File.ReadAllText(@"0:\etc\drive_mappings.conf"));

            bool hasRoot = false;

            _driveMappings = new();
            foreach (var map in root["drives"])
            {
                _driveMappings.Add(map["device"].Value, map["mount_point"].Value);
                if (map["mount_point"].Value == "/")
                    hasRoot = true;
            }

            if (!hasRoot)
            {
                _driveMappings.Add(@"0:\", "/");    // default mapping, just in case if not exists
            }
            ShellUtils.MoveCursorUp();
            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.OK, "DMM");
        }

        // The higher entry in mapping table, the more chances it will be maped
        // Example: If root map ('/') is on top, every other entry will be ignored
        // The best case is to place nested entries on top
        public static string MapFHSToPhysical(string path)
        {
            foreach(var mapping in _driveMappings)
            {
                if(path.StartsWith(mapping.Value, System.StringComparison.OrdinalIgnoreCase))
                {
                    string relativePath = mapping.Key + path[mapping.Value.Length..].Replace('/', '\\');
                    return relativePath;
                }
            }

            Logger.DoOSLog("[Warn] Did not found mapping for '" + path + "'");
            return path.Replace('/', '\\');
        }

        // The higher entry in mapping table, the more chances it will be maped
        // Example: If root map ('/') is on top, every other entry will be ignored
        // The best case is to place nested entries on top
        public static string MapPhysicalToFHS(string path)
        {
            foreach (var mapping in _driveMappings)
            {
                if(path.StartsWith(mapping.Key, System.StringComparison.OrdinalIgnoreCase))
                {
                    string relativePath = mapping.Value + path[mapping.Key.Length..].Replace('\\', '/');
                    return relativePath;
                }
            }

            Logger.DoOSLog("[Warn] Did not found mapping for '" + path + "'");

            return path.Replace("\\", "/");
        }

        public static void AddMapping(string phythical, string fhs)
        {
            _driveMappings.Add(phythical, fhs);
        }

        public static void RemoveMapping(string phythical)
        {
            _driveMappings.Remove(phythical);
        }

        public static void ReloadMapping()
        {
            if (!Directory.Exists(@"0:\etc"))
            {
                return;
            }

            if (!File.Exists(@"0:\etc\drive_mappings.conf"))
            {
                return;
            }

            DataNode root = JSONReader.ReadFromString(File.ReadAllText(@"0:\etc\drive_mappings.conf"));

            bool hasRoot = false;

            _driveMappings = new();
            foreach (var map in root["drives"])
            {
                _driveMappings.Add(map["device"].Value, map["mount_point"].Value);
                if (map["mount_point"].Value == "/")
                    hasRoot = true;
            }

            if (!hasRoot)
            {
                _driveMappings.Add(@"0:\", "/");    // default mapping, just in case if not exists
            }
        }

        public static void SaveMapping()
        {
            DataNode root = DataNode.CreateArray("drives");
            foreach(var map in _driveMappings)
            {
                DataNode driveNode = DataNode.CreateObject();
                driveNode.AddField("device", map.Key);
                driveNode.AddField("mount_point", map.Value);

                root.AddNode(driveNode);
            }

            string json = JSONWriter.WriteToString(root);

            // Don't trust yourself
            File.WriteAllText(@"0:\etc\drive_mappings.conf", json);
        }
    }
}
