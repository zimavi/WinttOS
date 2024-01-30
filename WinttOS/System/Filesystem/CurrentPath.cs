using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core;

namespace WinttOS.System.Filesystem
{
    public class CurrentPath
    {
        public static bool Set(string dir, out string error)
        {
            if (dir == "..")
            {
                Directory.SetCurrentDirectory(GlobalData.CurrentDirectory);

                var root = GlobalData.FileSystem.GetDirectory(GlobalData.CurrentDirectory);

                if (GlobalData.CurrentDirectory != GlobalData.CurrentVolume)
                {
                    GlobalData.CurrentDirectory = root.mParent.mFullPath;
                }
            }
            else if (dir == GlobalData.CurrentVolume)
            {
                GlobalData.CurrentDirectory = GlobalData.CurrentVolume;
            }
            else
            {
                if (Directory.Exists(GlobalData.CurrentDirectory + dir))
                {
                    Directory.SetCurrentDirectory(GlobalData.CurrentDirectory);
                    GlobalData.CurrentDirectory = GlobalData.CurrentDirectory + dir + @"\";
                }
                else if (File.Exists(GlobalData.CurrentDirectory + dir))
                {
                    error = "This is a file.";
                    return false;
                }
                else
                {
                    error = "This directory doesn't exist!";
                    return false;
                }
            }

            error = "none";
            return true;
        }
    }
}
