using System;
using WinttOS.Core;
using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.FileSystem
{
    public class RmCommand : Command
    {
        public RmCommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"rm <path\to\dir\or\file> - deletes directory or file");
        }
        public override string Execute(string[] arguments)
        {
            if (arguments.Length >= 1)
            {
                try
                {
                    if (Kernel.ReadonlyFiles.Contains(@"0:\" + GlobalData.CurrentDirectory + string.Join(' ', arguments)))
                    {
                        return "Unable to delete readonly file!";
                    }
                    Cosmos.System.FileSystem.VFS.VFSManager.DeleteFile(@"0:\" + GlobalData.CurrentDirectory + string.Join(' ', arguments));
                }
                catch
                {
                    try
                    {
                        if (Kernel.ReadonlyDirectories.Contains(@"0:\" + GlobalData.CurrentDirectory + string.Join(' ', arguments)))
                        {
                            return "Unable to delete readonly directory!";
                        }
                        Cosmos.System.FileSystem.VFS.VFSManager.DeleteDirectory(@"0:\" + GlobalData.CurrentDirectory + string.Join(' ', arguments), true);
                    }
                    catch
                    {
                        return "Deleting failed!";
                    }
                }
            }
            else
            {
                Console.Write("Enter file/dir name: ");
                string str = Console.ReadLine();
                try
                {
                    Cosmos.System.FileSystem.VFS.VFSManager.DeleteFile(@"0:\" + GlobalData.CurrentDirectory + str);
                }
                catch
                {
                    try
                    {
                        Cosmos.System.FileSystem.VFS.VFSManager.DeleteDirectory(@"0:\" + GlobalData.CurrentDirectory + str, true);
                    }
                    catch
                    {
                        return "Deleting failed!";
                    }
                }
            }

            return "Deleted successfully!";
        }
    }
}
