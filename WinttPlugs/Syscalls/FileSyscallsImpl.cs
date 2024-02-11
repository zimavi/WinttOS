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
        public static FileStream Open(string path, FileMode mode)
        {
            return Open(path, mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite), FileShare.None);
        }

        public static FileStream Open(string path, FileMode mode, FileAccess access)
        {
            return Open(path, mode, access, FileShare.None);
        }

        public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            if(mode == (FileMode.Truncate | FileMode.CreateNew | FileMode.Create | FileMode.OpenOrCreate | FileMode.Truncate | FileMode.Append) ||
                access == (FileAccess.ReadWrite | FileAccess.Write))
            {
                if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
                {
                    throw new FileModifyPermissionException(path);
                }
            }
            return new FileStream(path, mode, access, share);
        }

        public static void Delete(string path)
        {
            if(!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException(path);
            }
            File.Delete(path);
        }

        public static FileStream Create(string path)
        {
            if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException(path);
            }
            return File.Create(path);
        }
        public static FileStream Create(string path, int bufferSize)
        {
            if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException(path);
            }
            return File.Create(path, bufferSize);
        }

        public static FileStream OpenWrite(string path)
        {
            return Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }
    }
}
