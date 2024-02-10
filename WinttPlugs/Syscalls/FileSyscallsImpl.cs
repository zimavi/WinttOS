using IL2CPU.API.Attribs;

namespace WinttPlugs.Syscalls
{
    using PS = WinttOS.wSystem.wAPI.PrivilegesSystem;
    using System.IO;
    using WinttOS.wSystem.wAPI.Exceptions;

    [Plug(Target = typeof(System.IO.File))]
    public class FileSyscallsImpl
    {
        public static void WriteAllText(string path, string? contents)
        {
            if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException(path);
            }
            File.WriteAllText(path, contents);
        }
        public static void AppendAllText(string path, string? contents)
        {
            if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException(path);
            }
            File.AppendAllText(path, contents);
        }
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException(path);
            }
            File.WriteAllBytes(path, bytes);
        }
    }
}
