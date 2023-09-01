using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class rmCommand : Command
    {
        public rmCommand(string name) : base(name) 
        {
            HelpCommandManager.addCommandUageStrToManager(@"rm <path\to\dir\or\file> - deletes directory or file");
        }
        public override string execute(string[] arguments)
        {
            if(arguments.Length >= 1)
            {
                try
                {
                    Cosmos.System.FileSystem.VFS.VFSManager.DeleteFile(@"0:\" + GlobalData.currDir + String.Join(' ', arguments));
                }
                catch(Exception ex)
                {
                    try
                    {
                        Cosmos.System.FileSystem.VFS.VFSManager.DeleteDirectory(@"0:\" + GlobalData.currDir + arguments[0], true);
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
