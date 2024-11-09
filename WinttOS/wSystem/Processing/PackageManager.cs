using acryptohashnet;
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
using WinttOS.wSystem.Shell.Utils;
using WinttOS.wSystem.Utils;

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
                            Checksum = objects["checksum"]?.Value,       // this could be empty due to old repos
                            Author = objects["author"].Value,
                            Link = objects["link"].Value,
                            Version = objects["version"].Value,
                            Type = objects["type"]?.Value,
                        };

                        if (objects["dependencies"] != null)
                        {
                            foreach(var dependency in objects["dependencies"])
                            {
                                package.Dependencies.Add(dependency.Value);
                            }
                        }

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
                LocalRepository.Clear();

                foreach (string repoUrl in Repositories)
                {
                    SystemIO.STDOUT.PutLine($"Updating from '{repoUrl}'...");

                    Stopwatch sw = new();

                    sw.Start();
                    string json = Http.DownloadFile(repoUrl);

                    Logger.DoOSLog("[Info] Downloaded repo json");
                    //Logger.DoOSLog("[Info] " + json);

                    DataNode root;

                    try
                    {
                        root = JSONReader.ReadFromString(json);
                    }
                    catch (Exception ex)
                    {
                        SystemIO.STDOUT.PutLine("Cannot read JSON (" + ex.Message + "). Canceling repository update");
                        return;
                    }

                    Logger.DoOSLog("[Info] Parsed json");

                    foreach(DataNode objects in root)
                    {
                        // I didn't want to do this, but othervise GPF is thrown :/

                        Package package;

                        if (objects["checksum"] == null)
                        {
                            package = new Package
                            {
                                Installed = false,
                                Name = objects["name"].Value,
                                DisplayName = objects["display-name"].Value,
                                Description = objects["description"].Value,
                                Author = objects["author"].Value,
                                Checksum = null,
                                Link = objects["link"].Value,
                                Version = objects["version"].Value,
                                Type = objects["type"]?.Value,
                            };

                            if (objects["dependencies"] != null)
                            {
                                foreach (var dependency in objects["dependencies"])
                                {
                                    package.Dependencies.Add(dependency.Value);
                                }
                            }
                        }
                        else
                        {
                            package = new Package
                            {
                                Installed = false,
                                Name = objects["name"].Value,
                                DisplayName = objects["display-name"].Value,
                                Description = objects["description"].Value,
                                Author = objects["author"].Value,
                                Checksum = objects["checksum"].Value,
                                Link = objects["link"].Value,
                                Version = objects["version"].Value,
                                Type = objects["type"]?.Value,
                            };

                            if (objects["dependencies"] != null)
                            {
                                foreach (var dependency in objects["dependencies"])
                                {
                                    package.Dependencies.Add(dependency.Value);
                                }
                            }
                        }

                        string installPath;

                        if (package.Type == "application")
                            installPath = @"0:\usr\bin\" + package.Name + ".cexe";
                        else
                            installPath = @"0:\lib\" + package.Name + ".lib";

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

                    if(package.Executable.Dependencies.Count > 0)
                    {
                        SystemIO.STDOUT.PutLine("Resolving dependencies...");

                        Stopwatch sw2 = new();
                        sw2.Start();

                        List<string> foundDependencies = new();
                        foreach(var pkg in LocalRepository)
                        {
                            foreach(var dep in package.Dependencies)
                            {
                                if(dep.Equals(pkg.Name))
                                {
                                    SystemIO.STDOUT.PutLine("[OK] Found dependency " + pkg.Name + " in local repository");
                                    foundDependencies.Add(pkg.Name);
                                }
                            }

                            if (foundDependencies.Count >= package.Dependencies.Count)
                                break;
                        }

                        if(foundDependencies.Count < package.Dependencies.Count)
                        {
                            SystemIO.STDOUT.PutLine("E: Cannot resolve dependencies (not found in repository)");
                            Lists.CompareLists(package.Dependencies, foundDependencies, out List<string> notFound, out _);
                            SystemIO.STDOUT.PutLine("E: Missing dependencies: " + string.Join(", ", notFound.ToArray()));
                            return;
                        }

                        sw.Stop();
                        SystemIO.STDOUT.PutLine("[OK] Dependencies resolved. Took " + sw.TimeElapsed);

                        SystemIO.STDOUT.PutLine("Installing dependencies...");

                        foreach (var pkg in package.Dependencies)
                        {
                            Install(pkg);
                        }
                    }

                    bool isLib = package.Executable.IsLibrary;

                    if(isLib && Directory.Exists(@"0:\lib"))
                    {
                        Logger.DoOSLog("[OK] Installing package " + packageName);

                        SystemIO.STDOUT.PutLine("Downloaded package '" + package.Name + "' to '0:\\lib\\" + package.Name + ".lib'...");

                        File.WriteAllBytes(@"0:\lib\" + package.Name + ".lib", package.Executable.RawData);

                        SystemIO.STDOUT.PutLine("Validating file integrity...");

                        if (package.Checksum.IsNullOrWhiteSpace())
                        {
                            SystemIO.STDOUT.PutLine("W: No checksum present, skipping check...");
                            SystemIO.STDOUT.PutLine("W: Its NOT reccomended to execute this package is it cannot be validated!");
                        }
                        else
                        {
                            SHA256 hashing = new();

                            string checksum = hashing.ComputeHash(package.Executable.RawData).Hex();

                            // Cuttoff last 8 characters as SHA256 produce 4 empty bytes in os
                            // Same function used in executable creator, where are no empty bytes
                            if (!package.Checksum[..^8].Equals(checksum[..^8]))
                            {
                                SystemIO.STDOUT.PutLine("E: Checksum validation failed, aborting installation");
                                Logger.DoOSLog("Expected '" + package.Checksum[..^8] + "' produced '" + checksum[..^8] + "'");

                                Packages.Remove(package);
                                package.Installed = false;
                                File.Delete(@"0:\lib\" + package.Name + ".lib");

                                return;
                            }

                            SystemIO.STDOUT.PutLine("Package validation complete");
                        }

                        sw.Stop();
                        SystemIO.STDOUT.PutLine($"{packageName} installed (took {sw.TimeElapsed})");
                    }
                    else if (Directory.Exists(@"0:\usr\bin"))
                    {
                        Logger.DoOSLog("[OK] Installing package " + packageName);

                        SystemIO.STDOUT.PutLine("Downloaded package '" + package.Name + "' to '0:\\usr\\bin\\" + package.Name + ".cexe'...");

                        File.WriteAllBytes(@"0:\usr\bin\" + package.Name + ".cexe", package.Executable.RawData);

                        SystemIO.STDOUT.PutLine("Validating file integrity...");

                        if (package.Checksum.IsNullOrWhiteSpace())
                        {
                            SystemIO.STDOUT.PutLine("W: No checksum present, skipping check...");
                            SystemIO.STDOUT.PutLine("W: Its NOT reccomended to execute this package is it cannot be validated!");
                        }
                        else
                        {
                            SHA256 hashing = new();

                            string checksum = hashing.ComputeHash(package.Executable.RawData).Hex();

                            // Cuttoff last 8 characters as SHA256 produce 4 empty bytes in os
                            // Same function used in executable creator, where are no empty bytes
                            if (!package.Checksum[..^8].Equals(checksum[..^8]))
                            {
                                SystemIO.STDOUT.PutLine("E: Checksum validation failed, aborting installation");
                                Logger.DoOSLog("Expected '" + package.Checksum[..^8] + "' produced '" + checksum[..^8] + "'");

                                Packages.Remove(package);
                                package.Installed = false;
                                File.Delete(@"0:\usr\bin\" + package.Name + ".cexe");

                                return;
                            }

                            SystemIO.STDOUT.PutLine("Package validation complete");
                        }

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
                        if (package.Type == "application")
                            File.Delete(@"0:\usr\bin\" + package.Name + ".cexe");
                        else
                            File.Delete(@"0:\lib\" + packageName + ".lib");
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

            DataNode packages = DataNode.CreateArray();

            foreach (var package in LocalRepository)
            {
                var node = DataNode.CreateObject();

                node.AddField("name", package.Name);
                node.AddField("display-name", package.DisplayName);
                node.AddField("description", package.Description);
                node.AddField("author", package.Author);
                node.AddField("checksum", package.Checksum);
                node.AddField("link", package.Link);
                node.AddField("version", package.Version);
                node.AddField("type", package.Type);

                var depends = DataNode.CreateArray("dependencies");

                foreach (var dep in package.Dependencies)
                    depends.AddValue(dep);

                node.AddNode(depends);

                Logger.DoOSLog("[Info] Pkg -> Storing node");
                packages.AddNode(node);
            }

            var json = JSONWriter.WriteToString(packages);

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
