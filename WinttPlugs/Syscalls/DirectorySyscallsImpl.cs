using IL2CPU.API.Attribs;

namespace WinttPlugs.Syscalls
{
    using System.IO;
    using WinttOS.wSystem.wAPI.Exceptions;
    using PS = WinttOS.wSystem.wAPI.PrivilegesSystem;

    [Plug(Target = typeof(Directory))]
    public class DirectorySyscallsImpl
    {
        public static DirectoryInfo CreateDirectory(string path)
        {
            if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException(path);
            }
            return Directory.CreateDirectory(path);
        }
        public static void Delete(string path)
        {
            Delete(path, false);
        }

        public static void Delete(string path, bool recursive)
        {
            if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException(path);
            }
            Directory.Delete(path, recursive);
        }
    }
}
