using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Commands;

namespace WinttOS.Core.commands
{
    public class rmCommand : Command
    {
        public rmCommand(string name) : base(name, Users.User.AccessLevel.Guest) 
        {
            HelpCommandManager.addCommandUsageStrToManager(@"rm <path\to\dir\or\file> - deletes directory or file");
        }
        public override string execute(string[] arguments)
        {
            if(arguments.Length >= 1)
            {
                try
                {
                    if (Kernel.ReadonlyFiles.Contains(@"0:\" + GlobalData.currDir + String.Join(' ', arguments)))
                    {
                        return "Unable to delete readonly file!";
                    }    
                    Cosmos.System.FileSystem.VFS.VFSManager.DeleteFile(@"0:\" + GlobalData.currDir + String.Join(' ', arguments));
                }
                catch(Exception ex)
                {
                    try
                    {
                        if(Kernel.ReadonlyDirectories.Contains(@"0:\" + GlobalData.currDir + String.Join(' ', arguments)))
                        {
                            return "Unable to delete readonly directory!";
                        }
                        Cosmos.System.FileSystem.VFS.VFSManager.DeleteDirectory(@"0:\" + GlobalData.currDir + String.Join(' ', arguments), true);
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
                    Cosmos.System.FileSystem.VFS.VFSManager.DeleteFile(@"0:\" + GlobalData.currDir + str);
                }
                catch (Exception ex)
                {
                    try
                    {
                        Cosmos.System.FileSystem.VFS.VFSManager.DeleteDirectory(@"0:\" + GlobalData.currDir + str, true);
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
