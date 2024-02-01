using System;
using System.Collections.Generic;
using WinttOS.Core;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.FileSystem
{
    public class RmCommand : Command
    {
        public RmCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"rm <path\to\dir\or\file> - deletes directory or file");
        }
        public override ReturnInfo Execute(List<string> arguments)
        {
            try
            {
                if (Kernel.ReadonlyFiles.Contains(GlobalData.CurrentDirectory + string.Join(' ', arguments)))
                {
                    return new(this, ReturnCode.ERROR, "Unable to delete readonly file!");
                }
                Cosmos.System.FileSystem.VFS.VFSManager.DeleteFile(GlobalData.CurrentDirectory + string.Join(' ', arguments));
            }
            catch
            {
                try
                {
                    if (Kernel.ReadonlyDirectories.Contains(GlobalData.CurrentDirectory + string.Join(' ', arguments)))
                    {
                        return new(this, ReturnCode.ERROR, "Unable to delete readonly directory!");
                    }
                    Cosmos.System.FileSystem.VFS.VFSManager.DeleteDirectory(GlobalData.CurrentDirectory + string.Join(' ', arguments), true);
                }
                catch
                {
                    return new(this, ReturnCode.ERROR, "Deleting failed!");
                }
            }

            return new(this, ReturnCode.OK);
        }
    }
}
