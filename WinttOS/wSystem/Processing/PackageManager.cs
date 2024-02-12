using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using LunarLabs.Parser.JSON;
using LunarLabs.Parser;
using WinttOS.wSystem.Benchmark;

namespace WinttOS.wSystem.Processing
{
    public class PackageManager
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
        }

        public void Update()
        {
            try
            {
                foreach (string repoUrl in Repositories)
                {
                    Console.WriteLine($"Updating from '{repoUrl}'...");

                    Stopwatch sw = new();

                    sw.Start();
                    //string json = Http.DownloadFile(repoUrl);

                    string json = "[{\"name\":\"helloworld\",\"display-name\":\"Hello World\",\"description\":\"Test Lua\",\"author\":\"valentinbreiz\",\"link\":\"nope :)\",\"version\":\"1.0\"},{\"name\":\"hash\",\"display-name\":\"Hash\",\"description\":\"hash with lua\",\"author\":\"valentinbreiz\",\"link\":\"nope :)\",\"version\":\"1.0\"}]";

                    WinttDebugger.Info($"Downloaded json '{json}'", this);
                    
                    /*
                    JsonArray array = null;

                    using (var rdr = new JsonReader(json))
                    {
                        rdr.Parse();

                        WinttDebugger.Info("Parsed JSON", this);

                        array = rdr.GetArray();
                    }

                    for (int i = 0; i < array.Objects.Count; i++)
                    {
                        WinttDebugger.Info("Handling other package!", this);

                        var package = new Package();
                        package.Installed = false;

                        for(int j = 0; j < array[i].Count; i++)
                        {
                            WinttDebugger.Info("Handling properties", this);

                            string propertyName, propertyValue;

                            (propertyName, propertyValue) = array[i][j];

                            switch (propertyName)
                            {
                                case "name":
                                    package.Name = propertyValue;
                                    break;
                                case "display-name":
                                    package.DisplayName = propertyValue;
                                    break;
                                case "description":
                                    package.Description = propertyValue;
                                    break;
                                case "author":
                                    package.Author = propertyValue;
                                    break;
                                case "link":
                                    package.Link = propertyValue;
                                    break;
                                case "version":
                                    package.Version = propertyValue;
                                    break;
                            }
                        }

                        LocalRepository.Add(package);
                    }
                    */

                    var root = JSONReader.ReadFromString(json);
                    
                    foreach(DataNode objects in root)
                    {
                        WinttDebugger.Info("Handling other package!", this);

                        var package = new Package();
                        package.Installed = false;

                        WinttDebugger.Info("Assinging variables!", this);

                        package.Name = objects["name"].Value;
                        package.DisplayName = objects["display-name"].Value;
                        package.Description = objects["description"].Value;
                        package.Author = objects["author"].Value;
                        package.Link = objects["link"].Value;
                        package.Version = objects["version"].Value;

                        LocalRepository.Add(package);
                    }
                    sw.Stop();
                    Console.WriteLine($"Done, took {sw.TimeElapsed}");
                }

                Console.WriteLine("Done.");
            }
            catch (Exception e)
            {
                WinttDebugger.Error(e.Message, true);
                Kernel.WinttRaiseHardError(Core.Utils.Kernel.WinttStatus.SYSTEM_SERVICE_EXCEPTION, this, Core.Utils.Kernel.HardErrorResponseOption.OptionShutdownSystem);
            }
        }

        public void Upgrade()
        {
            Console.WriteLine("Upgrading packages...");

            bool upgraded = false;
            
            foreach(var package in Packages)
            {
                Stopwatch sw = new();
                sw.Start();

                Console.Write($"- '{package.Link}' ");
                package.Download();

                sw.Stop();
                Console.WriteLine($"[OK] (took {sw.TimeElapsed})");

                upgraded = true;
            }

            if (!upgraded)
            {
                Console.WriteLine("No package found.");
            }
        }

        public void AddRepo(string url)
        {
            if (url.StartsWith("https://"))
            {
                Console.WriteLine("HTTPS is not supported yet, please use HTTP");
                return;
            }

            Repositories.Add(url);
            Console.WriteLine("Done.");
        }

        public void RemoveRepo(int id)
        {
            if(id < 0 || id > Repositories.Count - 1)
            {
                Console.WriteLine("Error: id is out of bounce");
                return;
            }
            Repositories.RemoveAt(id);

            Console.WriteLine("Done.");
        }

        public void Install(string packageName)
        {
            WinttDebugger.Trace($"Installing package '{packageName}'");
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

                    WinttDebugger.Trace("Installed!");

                    Console.WriteLine($"{packageName} installed (took {sw.TimeElapsed})");

                    return;
                }
            }
            Console.WriteLine($"{packageName} not found!");
        }

        public void Remove(string packageName)
        {
            foreach (var package in LocalRepository)
            {
                if (package.Name == packageName)
                {
                    package.Installed = false;
                    Packages.Remove(package);

                    Console.WriteLine($"{packageName} removed.");

                    return;
                }
            }

            Console.WriteLine($"{packageName} not found.");
        }
    }
}
