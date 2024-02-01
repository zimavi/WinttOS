using JZero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.wSystem.Networking;

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
            foreach(string repoUrl in Repositories)
            {
                Console.WriteLine($"Updating from '{repoUrl}'...");

                string json = Http.DownloadFile(repoUrl);

                var rdr = new JsonReader(json);
                rdr.ReadArrayStart();
                {
                    while(rdr.NextElement())
                    {
                        var package = new Package();
                        package.Installed = false;

                        rdr.ReadObjectStart();
                        {
                            while (rdr.NextProperty())
                            {
                                var charSegment = rdr.ReadPropertyName();
                                var charSegment2 = rdr.ReadString();

                                string propName = new string(charSegment.Array, charSegment.Offset, charSegment.Count);
                                string propValue = new string(charSegment2.Array, charSegment2.Offset, charSegment2.Count);
                                
                                switch(propName)
                                {
                                    case "name":
                                        package.Name = propValue;
                                        break;
                                    case "display-name":
                                        package.DisplayName = propValue;
                                        break;
                                    case "description":
                                        package.Description = propValue;
                                        break;
                                    case "author":
                                        package.Author = propValue;
                                        break;
                                    case "link":
                                        package.Link = propValue;
                                        break;
                                    case "version":
                                        package.Version = propValue;
                                        break;
                                }
                            }
                        }

                        LocalRepository.Add(package);
                    }
                }
            }

            Console.WriteLine("Done.");
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
