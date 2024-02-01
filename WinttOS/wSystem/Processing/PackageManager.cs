using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Networking;
using WinttOS.wSystem.Json;

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

                    string json = Http.DownloadFile(repoUrl);

                    WinttDebugger.Info($"Downloaded json '{json}'", this);

                    /*
                    var rdr = new JsonReader(json);

                    WinttDebugger.Info("Created Reader", this);

                    rdr.ReadArrayStart();
                    {
                        while (rdr.NextElement())
                        {
                            var package = new Package();
                            package.Installed = false;

                            rdr.ReadObjectStart();
                            {
                                while (rdr.NextProperty())
                                {
                                    var charSegment = rdr.ReadPropertyName();
                                    var charSegment2 = rdr.ReadString();

                                    string propertyName = new string(charSegment.Array, charSegment.Offset, charSegment.Count);
                                    string propertyValue = new string(charSegment2.Array, charSegment2.Offset, charSegment2.Count);
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
                            }

                            LocalRepository.Add(package);

                        }
                    }
                    */

                    JsonArray array = null;

                    using (var rdr = new JsonReader(json))
                    {
                        rdr.Parse();

                        array = rdr.GetArray();
                    }

                    for (int i = 0; i < array.Objects.Count; i++)
                    {
                        var package = new Package();
                        package.Installed = false;

                        for(int j = 0; j < array[i].Count; i++)
                        {
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
                Console.Write($"- '{package.Link}' ");
                package.Download();
                Console.WriteLine("[OK]");

                upgraded = true;
            }

            if (!upgraded)
            {
                Console.WriteLine("No package found.");
            }
        }

        public void AddRepo(string url)
        {
            if(url.StartsWith("https://"))
            {
                Console.WriteLine("HTTPS is not suppoerted yet, please use HTTP");
                return;
            }

            Repositories.Add(url);
            Console.WriteLine("Done.");
        }

        public void Install(string packageName)
        {
            foreach(var package in LocalRepository)
            {
                if (package.Name == packageName)
                {
                    package.Installed = true;
                    Packages.Add(package);
                    package.Download();

                    Console.WriteLine($"{packageName}  added.");

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
