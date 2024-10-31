using System.IO;
using WinttOS.Core;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.wSystem.Filesystem
{
    public sealed class CurrentPath
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
                    Logger.DoOSLog("[Warn] Change dir -> Directory '" + GlobalData.CurrentDirectory + dir + "' does not exists!");
                    error = "This directory doesn't exist!";
                    return false;
                }
            }

            error = "none";
            return true;
        }
    }
}
