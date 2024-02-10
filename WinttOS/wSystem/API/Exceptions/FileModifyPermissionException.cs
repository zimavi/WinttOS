using System;

namespace WinttOS.wSystem.wAPI.Exceptions
{
    public class FileModifyPermissionException : Exception
    {
        public readonly string Path;

        public FileModifyPermissionException(string path) : base("You do not have permission to modify to this file.")
        {
            Path = path;
        }
    }
}
