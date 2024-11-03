using LunarLabs.Parser;
using LunarLabs.Parser.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Benchmark;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Networking;

namespace WinttOS.wSystem.Processing
{
    public static class PackageManager
    {
        public static List<string> Repositories { get; set; }
        public static List<Package> LocalRepository { get; set; }
        public static List<Package> Packages { get; set; }

        public static void Initialize()
        {
            Logger.DoOSLog("[Info] Initializing PackageManager");
            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.NONE, "PackageManager");

            Repositories = new List<string>
            {
                "http://winttos.localto.net/repository.json"
            };
            Logger.DoOSLog("[Info] Repository initialized: " + string.Join(", ", Repositories.ToArray()));

            LocalRepository = new();
            Packages = new();

            ShellUtils.PrintTaskResult("Loading", ShellTaskResult.DOING, "Local Repository");
            try
            {
                if (File.Exists(@"0:\etc\packos\repositories.json"))
                {
                    var root = JSONReader.ReadFromString(File.ReadAllText(@"0:\etc\packos\repositories.json"));
                    foreach (DataNode objects in root)
                    {

                        var package = new Package
                        {
                            Installed = false,
                            Name = objects["name"].Value,
                            DisplayName = objects["display-name"].Value,
                            Description = objects["description"].Value,
                            Author = objects["author"].Value,
                            Link = objects["link"].Value,
                            Version = objects["version"].Value
                        };

                        string installPath = @"0:\usr\bin\" + package.Name + ".cexe";

                        if (File.Exists(installPath))
                        {
                            package.Installed = true;
                            Packages.Add(package);
                        }

                        LocalRepository.Add(package);
                    }
                    ShellUtils.MoveCursorUp();
                    ShellUtils.PrintTaskResult("Loading", ShellTaskResult.OK, "Local Repository");
                    return;
                }
                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.OK, "Local Repository: Not Found");
            } 
            catch (Exception)
            {
                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "Local Repository");
                SystemIO.STDOUT.PutLine("Cannot load '\\etc\\packos\\repositories.json': Invalid JSON");

            }
        }

        public static void Update()
        {
            try
            {
                foreach (string repoUrl in Repositories)
                {
                    SystemIO.STDOUT.PutLine($"Updating from '{repoUrl}'...");

                    Stopwatch sw = new();

                    sw.Start();
                    string json = Http.DownloadFile(repoUrl);

                    Logger.DoOSLog("[Info] Downloaded repo json");

                    var root = JSONReader.ReadFromString(json);
                    
                    foreach(DataNode objects in root)
                    {

                        var package = new Package
                        {
                            Installed = false,
                            Name = objects["name"].Value,
                            DisplayName = objects["display-name"].Value,
                            Description = objects["description"].Value,
                            Author = objects["author"].Value,
                            Link = objects["link"].Value,
                            Version = objects["version"].Value
                        };

                        string installPath = @"0:\usr\bin\" + package.Name + ".cexe";

                        if (File.Exists(installPath))
                        {
                            package.Installed = true;
                            Packages.Add(package);
                        }

                        LocalRepository.Add(package);
                    }
                    UpdateFiles();

                    sw.Stop();
                    SystemIO.STDOUT.PutLine($"Done, took {sw.TimeElapsed}");

                }

                SystemIO.STDOUT.PutLine("Done.");
            }
            catch (Exception e)
            {
                Logger.DoOSLog("[Error] Cannot update repository (" + e.Message + ")");
            }
        }

        public static void Upgrade()
        {
            SystemIO.STDOUT.PutLine("Upgrading packages...");

            bool upgraded = false;
            
            foreach(var package in Packages)
            {
                Stopwatch sw = new();
                sw.Start();

                Console.Write($"- '{package.Link}' ");
                package.Download();

                sw.Stop();
                SystemIO.STDOUT.PutLine($"[OK] (took {sw.TimeElapsed})");

                upgraded = true;
            }

            if (!upgraded)
            {
                SystemIO.STDOUT.PutLine("No package found.");
            }
        }

        public static void AddRepo(string url)
        {
            if (url.StartsWith("https://"))
            {
                SystemIO.STDOUT.PutLine("HTTPS is not supported yet, please use HTTP");
                return;
            }

            Repositories.Add(url);
            SystemIO.STDOUT.PutLine("Done.");
        }

        public static void RemoveRepo(int id)
        {
            if(id < 0 || id > Repositories.Count - 1)
            {
                SystemIO.STDOUT.PutLine("Error: id is out of bounce");
                return;
            }
            Repositories.RemoveAt(id);

            SystemIO.STDOUT.PutLine("Done.");
        }

        public static void Install(string packageName)
        {
            Logger.DoOSLog("[Info] Installing package " + packageName);
            foreach(var package in LocalRepository)
            {
                if (package.Name == packageName)
                {
                    Stopwatch sw = new();

                    SystemIO.STDOUT.PutLine("Installing '" + package.Name + "' from '" + package.Link + "'...");
                    sw.Start();

                    package.Installed = true;
                    Packages.Add(package);
                    package.Download();

                    if(Directory.Exists(@"0:\usr\bin"))
                    {
                        Logger.DoOSLog("[OK] Installing package " + packageName);

                        File.WriteAllBytes(@"0:\usr\bin\" + package.Name + ".cexe", package.Executable.RawData);

                        sw.Stop();
                        SystemIO.STDOUT.PutLine($"{packageName} installed (took {sw.TimeElapsed})");
                    }
                    else
                    {
                        SystemIO.STDOUT.PutLine(packageName + " added");
                    }

                    return;
                }
            }
            SystemIO.STDOUT.PutLine($"{packageName} not found!");
        }

        public static void Remove(string packageName)
        {
            foreach (var package in LocalRepository)
            {
                if (package.Name == packageName)
                {
                    package.Installed = false;
                    Packages.Remove(package);
                    try
                    {
                        File.Delete(@"0:\usr\bin\" + package.Name + ".cexe");
                    }
                    catch (Exception ex)
                    {
                        Logger.DoOSLog("[Error] Cannot delete package -> '" + package.Name + "' -> " + ex.Message);
                    }

                    SystemIO.STDOUT.PutLine($"{packageName} removed.");

                    return;
                }
            }

            SystemIO.STDOUT.PutLine($"{packageName} not found.");
        }

        public static void UpdateFiles()
        {
            Logger.DoOSLog("[Info] Pkg -> Updating files");

            DataNode root = DataNode.CreateArray();

            foreach (var package in LocalRepository)
            {
                var node = DataNode.CreateObject();

                node.AddField("name", package.Name);
                node.AddField("display-name", package.DisplayName);
                node.AddField("description", package.Description);
                node.AddField("author", package.Author);
                node.AddField("link", package.Link);
                node.AddField("version", package.Version);

                Logger.DoOSLog("[Info] Pkg -> Storing node");
                root.AddNode(node);
            }

            var json = JSONWriter.WriteToString(root);

            if (!Directory.Exists(@"0:\etc\packos"))
                Directory.CreateDirectory(@"0:\etc\packos");
            try
            {
                File.WriteAllText(@"0:\etc\packos\repositories.json", json);
            }
            catch(Exception ex)
            {
                Logger.DoOSLog("[Error] Pkf -> Cannot save file -> '" + ex.Message + "'");
            }

            Logger.DoOSLog("[OK] Pkg -> Files updated");
        }
    }
}
