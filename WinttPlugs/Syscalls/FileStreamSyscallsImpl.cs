using IL2CPU.API.Attribs;

namespace WinttPlugs.Syscalls
{
    using System.IO;
    using PS = WinttOS.wSystem.wAPI.PrivilegesSystem;
    using WinttOS.wSystem.wAPI.Exceptions;

    [Plug(Target = typeof(FileStream))]
    public static class FileStreamSyscallsImpl
    {
        public static void Write(this FileStream fileStream, byte[] buffer, int offset, int count)
        {
            if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException("FileStream");
            }
            fileStream.Write(buffer, offset, count);
        }

        public static void WriteByte(this FileStream fileStream, byte value)
        {
            if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException("FileStream");
            }
            fileStream.WriteByte(value);
        }

        public static void Flush(this FileStream fileStream)
        {
            Flush(fileStream, false);
        }
        public static void Flush(this FileStream fileStream, bool flushToDisk)
        {
            if (!PS.HasFlag(WinttOS.wSystem.WinttOS.CurrentExecutionSet, PS.Privileges.FILE_WRITE))
            {
                throw new FileModifyPermissionException("FileStream");
            }
            fileStream.Flush(flushToDisk);
        }
    }
}
