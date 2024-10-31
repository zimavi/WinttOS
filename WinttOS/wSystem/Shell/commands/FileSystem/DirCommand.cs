using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Shell.commands.FileSystem;
using WinttOS.wSystem.Shell.Utils;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public sealed class DirCommand : Command
    {
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

        public DirCommand(string[] name) : base(name, AccessLevel.Default)
        { }

        public override ReturnInfo Execute()
        {
            try
            {
                var di = new DirectoryInfo(GlobalData.CurrentDirectory);
                var dir_files = di.GetFileSystemInfos();

                ConsoleColor def_col;

                if (WinttOS.IsTty)
                {
                    def_col = WinttOS.Tty.Foreground;
                }
                else
                    def_col = Console.ForegroundColor;

                ConsoleColumnFormatter fmt;

                if (WinttOS.IsTty)
                {
                    fmt = new(20, (byte)(WinttOS.Tty.Cols / 10));
                }
                else
                    fmt = new(20, (byte)Console.WindowWidth);

                foreach (var file in dir_files)
                {
                    if (file.IsDirectory())
                    {
                        if (file.Name.StartsWith('.'))
                            continue;

                        if (WinttOS.IsTty)
                            WinttOS.Tty.Foreground = ConsoleColor.Blue;
                        else
                            Console.ForegroundColor = ConsoleColor.Blue;

                        fmt.Write(file.Name);
                    }
                    else
                    {
                        string ext = file.Extension.ToLower();
                        if (ExeExts.Contains(ext))
                        {
                            if (WinttOS.IsTty)
                                WinttOS.Tty.Foreground = ConsoleColor.Red;
                            else
                                Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (ArchExts.Contains(ext))
                        {
                            if (WinttOS.IsTty)
                                WinttOS.Tty.Foreground = ConsoleColor.Green;
                            else
                                Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            if (WinttOS.IsTty)
                                WinttOS.Tty.Foreground = def_col;
                            else
                                Console.ForegroundColor = def_col;
                        }

                        fmt.Write (file.Name);
                    }
                }
            }
            catch (Exception e)
            {
                SystemIO.STDOUT.PutLine(e.ToString() + "\n" + e.Message);
                SystemIO.STDOUT.PutLine("No files in directory");
            }

            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("dir");
        }
    }
}
