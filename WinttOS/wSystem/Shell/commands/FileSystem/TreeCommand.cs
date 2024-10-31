using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WinttOS.Core;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.FileSystem
{
    public class TreeCommand : Command
    {
        public TreeCommand(string[] commandValues) : base(commandValues)
        { }

        public override ReturnInfo Execute()
        {
            string startDir = GlobalData.CurrentDirectory;
            new Tree(startDir).Print();
            return new(this, ReturnCode.OK);
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            string startDir = GlobalData.CurrentDirectory;
            bool showHidden = false;
            int maxDepth = int.MaxValue;

            for(int i = 0; i < arguments.Count; i++)
            {
                string arg = arguments[i];

                if (arg == "--hidden" || arg == "-h")
                    showHidden = true;
                else if (arg == "--level" || arg == "-l")
                {
                    maxDepth = int.Parse(arguments[i + 1]);
                    i++;
                }
                else
                {
                    if(startDir == GlobalData.CurrentDirectory)
                    {
                        startDir = arg;
                    }
                    else
                    {
                        PrintHelp();
                        return new(this, ReturnCode.ERROR_ARG);
                    }
                }
            }

            new Tree(startDir)
            {
                ShowHidden = showHidden,
                MaxDepth = maxDepth,
            }.Print();

            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("tree");
            SystemIO.STDOUT.PutLine("tree [--hidden | -h]");
            SystemIO.STDOUT.PutLine("tree [--level  | -l] level");
        }
    }

    public class Tree
    {
        public Tree(string startDir) 
        {
            StartDir = startDir;

            if (WinttOS.IsTty)
            {
                DefaultColor = WinttOS.Tty.Foreground;
                FileColor = WinttOS.Tty.Foreground;
            }
            else
            {
                DefaultColor = Console.ForegroundColor;
                FileColor = Console.ForegroundColor;
            }
        }
        public string StartDir { get; }
        public bool ShowHidden { get; set; } = false;
        public int MaxDepth { get; set; } = int.MaxValue;
        public ConsoleColor DefaultColor { get; set; }
        public ConsoleColor DirColor { get; set; } = ConsoleColor.Blue;
        public ConsoleColor FileColor { get; set; }
        public ConsoleColor ExecutableColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor ArchiveColor { get; set; } = ConsoleColor.Green;
        public HashSet<string> ExeExts { get; set; } = new() { ".cexe", ".sh" };
        public HashSet<string> ArchExts { get; set; } = new() 
        { 
            ".zip",
            ".7z",
            ".gz",
            ".tar",
            ".iso",
            ".br",
            ".br2",
            ".genozip",
            ".lz",
            ".lz4",
            ".lzma",
            ".lzo",
            ".rz",
            ".sz",
            ".xz",
            ".z",
            ".zst",
            ".s7z",
            ".cab",
            ".jar",
            ".zipx"
        };

        public void Print()
        {
            WriteColored(StartDir, DirColor);
            SystemIO.STDOUT.PutLine("");

            PrintTree(StartDir);
        }

        private void WriteColored(string text, ConsoleColor color)
        {
            if (WinttOS.IsTty)
            {
                WinttOS.Tty.Foreground = color;
                SystemIO.STDOUT.Put(text);
                WinttOS.Tty.Foreground = DefaultColor;
            }
            else
            {
                Console.ForegroundColor = color;
                SystemIO.STDOUT.Put(text);
                Console.ForegroundColor = DefaultColor;
            }
        }

        private ConsoleColor GetColor(FileSystemInfo fsInfo)
        {
            if (fsInfo.IsDirectory())
            {
                return DirColor;
            }
            string ext = fsInfo.Extension.ToLower();
            if (ExeExts.Contains(ext))
            {
                return ExecutableColor;
            }
            else if (ArchExts.Contains(ext))
            {
                return ArchiveColor;
            }
            return FileColor;
        }

        private void WriteName(FileSystemInfo fsInfo)
        {
            WriteColored(fsInfo.Name, GetColor(fsInfo));
        }

        private void PrintTree(string startDir, string prefix = "", int depth = 0)
        {
            if(depth >= MaxDepth)
            {
                return;
            }

            var di = new DirectoryInfo(startDir);
            var fsItems = di.GetFileSystemInfos()
                .Where(f => ShowHidden || !f.Name.StartsWith("."))
                .ToList();

            foreach(var fsItem in fsItems.Take(fsItems.Count - 1)) 
            {
                SystemIO.STDOUT.Put(prefix + new string(new char[] { (char)0x0c6, (char)0x0c0, (char)0x0c0, ' ' }));  // "├── "
                WriteName(fsItem);
                SystemIO.STDOUT.PutLine("");
                if (fsItem.IsDirectory())
                {
                    PrintTree(fsItem.FullName, prefix + new string(new char[] { (char)0x0c1, ' ', ' ', ' ' }), depth + 1); // "│   "
                }
            }

            var lastFsItem = fsItems.LastOrDefault();
            if(lastFsItem != null)
            {
                SystemIO.STDOUT.Put(prefix + new string(new char[] { (char)0x0c4, (char)0x0c0, (char)0x0c0, ' ' } )); //"└── "
                WriteName(lastFsItem);
                SystemIO.STDOUT.PutLine("");
                if(lastFsItem.IsDirectory())
                {
                    PrintTree(lastFsItem.FullName, prefix + "    ", depth + 1);
                }
            }
        }
    }
    public static class FileSystemInfoExtensions
    {
        public static bool IsDirectory(this FileSystemInfo fsItem)
        {
            return (fsItem.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}
