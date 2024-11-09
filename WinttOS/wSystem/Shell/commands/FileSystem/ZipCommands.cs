using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.wSystem.Compression;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.FileSystem
{
    public class CreateZipCommand : Command
    {
        public CreateZipCommand() : base(new string[] { "createzip" })
        {
            Description = "Creates new zip file.";
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if(arguments.Count < 1) 
            {
                return new(this, ReturnCode.ERROR_ARG);
            }

            string zipPath = arguments[0];

            if (!zipPath.StartsWith('/') && !zipPath.StartsWith('\\'))
            {
                zipPath = GlobalData.CurrentDirectory + zipPath;
            }
            try
            {
                ZipStorer.Create(zipPath, "Created by ZipStorer using " + WinttOS.WinttVersion);
                SystemIO.STDOUT.PutLine("Zip file created successfully.");
                return new(this, ReturnCode.OK);
            }
            catch (Exception ex)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, ex.Message);
            }
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("createzip <zipfile>");
        }
    }

    public class UnzipCommand : Command
    {
        public UnzipCommand() : base(new string[] { "addtozip" })
        {
            Description = "Extract contents from a zip file.";
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments.Count < 2)
            {
                return new ReturnInfo(this, ReturnCode.ERROR_ARG);
            }

            string zipFileName = arguments[0];
            
            string destination = arguments[1];


            if (!zipFileName.StartsWith('/') && !zipFileName.StartsWith('\\'))
            {
                zipFileName = GlobalData.CurrentDirectory + zipFileName;
            }
            if (!destination.StartsWith('/') && !destination.StartsWith('\\'))
            {
                destination = GlobalData.CurrentDirectory + destination;
            }

            try
            {
                using (var zip = ZipStorer.Open(zipFileName, FileAccess.Read))
                {
                    var dir = zip.ReadCentralDir();
                    foreach (var entry in dir)
                    {
                        zip.ExtractFile(entry, Path.Combine(destination, entry.FilenameInZip));
                    }
                    return new ReturnInfo(this, ReturnCode.OK, "Contents extracted successfully.");
                }
            }
            catch (Exception ex)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, ex.Message);
            }
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("unzip <zipfile> <destination>");
        }
    }

    public class ListZipContentsCommand : Command
    {
        public ListZipContentsCommand() : base(new string[] { "listzip" })
        {
            Description = "List contents of a zip file.";
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments.Count < 1)
            {
                return new ReturnInfo(this, ReturnCode.ERROR_ARG);
            }

            string zipFileName = arguments[0];
            if (!zipFileName.StartsWith('/') && !zipFileName.StartsWith('\\'))
            {
                zipFileName = GlobalData.CurrentDirectory + zipFileName;
            }
            try
            {
                using (var zip = ZipStorer.Open(zipFileName, FileAccess.Read))
                {
                    var dir = zip.ReadCentralDir();
                    foreach (var entry in dir)
                    {
                        SystemIO.STDOUT.PutLine($"{entry.FilenameInZip} ({Filesystem.Utils.ConvertSize(entry.FileSize)})");
                    }
                    return new ReturnInfo(this, ReturnCode.OK, "Contents listed successfully.");
                }
            }
            catch (Exception ex)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, ex.Message);
            }
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("listzip <zipfile>");
        }
    }

    public class AddFileToZipCommand : Command
    {
        public AddFileToZipCommand() : base(new string[] { "addtozip" })
        {
            Description = "Add a file to an existing zip file.";
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments.Count < 2)
            {
                return new ReturnInfo(this, ReturnCode.ERROR_ARG);
            }

            string zipFileName = arguments[0];
            string fileToAdd = arguments[1];

            if (!zipFileName.StartsWith('/') && !zipFileName.StartsWith('\\'))
            {
                zipFileName = GlobalData.CurrentDirectory + zipFileName;
            }
            if (!fileToAdd.StartsWith('/') && !fileToAdd.StartsWith('\\'))
            {
                fileToAdd = GlobalData.CurrentDirectory + fileToAdd;
            }

            try
            {
                using (var zip = ZipStorer.Open(zipFileName, FileAccess.Write))
                {
                    zip.AddFile(ZipStorer.Compression.Store, fileToAdd, Path.GetFileName(fileToAdd), "");
                    return new ReturnInfo(this, ReturnCode.OK, "File added successfully.");
                }
            }
            catch (Exception ex)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, ex.Message);
            }
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("addtozip <zipfile> <file>");
        }
    }
}
