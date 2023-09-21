using System;
using WinttOS.Core;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands.FileSystem
{
    public class rmCommand : Command
    {
        public rmCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"rm <path\to\dir\or\file> - deletes directory or file");
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
                catch (Exception ex)
                {
                    try
                    {
                        if (Kernel.ReadonlyDirectories.Contains(@"0:\" + GlobalData.CurrentDirectory + string.Join(' ', arguments)))
                        {
                            return "Unable to delete readonly directory!";
                        }
                        Cosmos.System.FileSystem.VFS.VFSManager.DeleteDirectory(@"0:\" + GlobalData.CurrentDirectory + string.Join(' ', arguments), true);
                    }
                    catch (Exception ex2)
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
                catch (Exception ex)
                {
                    try
                    {
                        Cosmos.System.FileSystem.VFS.VFSManager.DeleteDirectory(@"0:\" + GlobalData.CurrentDirectory + str, true);
                    }
                    catch (Exception ex2)
                    {
                        return "Deleting failed!";
                    }
                }
            }

            return "Deleted successfully!";
        }
    }
}
