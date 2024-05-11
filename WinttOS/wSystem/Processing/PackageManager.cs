using LunarLabs.Parser;
using LunarLabs.Parser.JSON;
using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Benchmark;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Processing
{
    public sealed class PackageManager
    {
        public List<string> Repositories;
        public List<Package> LocalRepository;
        public List<Package> Packages;

        public void Initialize()
        {
            Repositories = new()
            {
                "http://winttos.localto.net/repository.json"
            };
            LocalRepository = new();
            Packages = new();
            ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.NONE, "PackageManager");
        }

        public void Update()
        {
            try
            {
                foreach (string repoUrl in Repositories)
                {
                    SystemIO.STDOUT.PutLine($"Updating from '{repoUrl}'...");

                    Stopwatch sw = new();

                    sw.Start();
                    //string json = Http.DownloadFile(repoUrl);

                    string json = "[{\"name\":\"helloworld\",\"display-name\":\"Hello World\",\"description\":\"Test Lua\",\"author\":\"valentinbreiz\",\"link\":\"nope :)\",\"version\":\"1.0\"},{\"name\":\"hash\",\"display-name\":\"Hash\",\"description\":\"hash with lua\",\"author\":\"valentinbreiz\",\"link\":\"nope :)\",\"version\":\"1.0\"}]";

                    Logger.DoOSLog("[Info] Downloaded repo json");
                    

                    var root = JSONReader.ReadFromString(json);
                    
                    foreach(DataNode objects in root)
                    {

                        var package = new Package
                        {
                            Installed = false
                        };

                        package.Name = objects["name"].Value;
                        package.DisplayName = objects["display-name"].Value;
                        package.Description = objects["description"].Value;
                        package.Author = objects["author"].Value;
                        package.Link = objects["link"].Value;
                        package.Version = objects["version"].Value;

                        LocalRepository.Add(package);
                    }
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

        public void Upgrade()
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

        public void AddRepo(string url)
        {
            if (url.StartsWith("https://"))
            {
                SystemIO.STDOUT.PutLine("HTTPS is not supported yet, please use HTTP");
                return;
            }

            Repositories.Add(url);
            SystemIO.STDOUT.PutLine("Done.");
        }

        public void RemoveRepo(int id)
        {
            if(id < 0 || id > Repositories.Count - 1)
            {
                SystemIO.STDOUT.PutLine("Error: id is out of bounce");
                return;
            }
            Repositories.RemoveAt(id);

            SystemIO.STDOUT.PutLine("Done.");
        }

        public void Install(string packageName)
        {
            Logger.DoOSLog("[Info] Installing package " + packageName);
            foreach(var package in LocalRepository)
            {
                if (package.Name == packageName)
                {
                    Stopwatch sw = new();
                    sw.Start();

                    package.Download();

                    if(package.Executable == null)
                    {
                        return;
                    }

                    package.Installed = true;

                    Packages.Add(package);

                    sw.Stop();

                    Logger.DoOSLog("[OK] Installing package " + packageName);

                    SystemIO.STDOUT.PutLine($"{packageName} installed (took {sw.TimeElapsed})");

                    return;
                }
            }
            SystemIO.STDOUT.PutLine($"{packageName} not found!");
        }

        public void Remove(string packageName)
        {
            foreach (var package in LocalRepository)
            {
                if (package.Name == packageName)
                {
                    package.Installed = false;
                    Packages.Remove(package);

                    SystemIO.STDOUT.PutLine($"{packageName} removed.");

                    return;
                }
            }

            SystemIO.STDOUT.PutLine($"{packageName} not found.");
        }
    }
}
