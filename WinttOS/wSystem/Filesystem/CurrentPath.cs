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
                Directory.SetCurrentDirectory(IOMapper.MapFHSToPhysical(GlobalData.CurrentDirectory));

                var root = GlobalData.FileSystem.GetDirectory(IOMapper.MapFHSToPhysical(GlobalData.CurrentDirectory));

                if (GlobalData.CurrentDirectory != "/")
                {
                    GlobalData.CurrentDirectory = IOMapper.MapPhysicalToFHS(root.mParent.mFullPath);
                }
            }
            else if (dir == "/")
            {
                GlobalData.CurrentDirectory = "/";
            }
            else
            {
                if (Directory.Exists(IOMapper.MapFHSToPhysical(GlobalData.CurrentDirectory + dir)))
                {
                    Directory.SetCurrentDirectory(GlobalData.CurrentDirectory);
                    GlobalData.CurrentDirectory = IOMapper.MapPhysicalToFHS(GlobalData.CurrentDirectory + dir + @"\");
                }
                else if (File.Exists(IOMapper.MapFHSToPhysical(GlobalData.CurrentDirectory + dir)))
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
