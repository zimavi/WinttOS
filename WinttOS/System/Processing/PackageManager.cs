﻿using JZero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.Networking;

namespace WinttOS.System.Processing
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

            // TODO: Finish it, when executables are implemented
        }
    }
}
