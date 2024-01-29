using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.System.Processing;

namespace WinttOS.System.API
{
    public static class IO
    {

        public static List<string> ReadonlyFiles { get; internal set; } = new() { };
        public static bool TryMarkReadonly(Process process, string path) 
        {
            if (process.CurrentSet.Value >= PrivilegesSystem.PrivilegesSet.DEFAULT.Value 
                && WinttOS.UsersManager.CurrentUser.UserAccess.Value >= Users.User.AccessLevel.Default.Value)
            {
                ReadonlyFiles.Add(path);
                return true;
            }
            return false;
        }
        public static bool TryReadFile(Process process, string path, out string contents)
        {
            if (process.CurrentSet.Value >= PrivilegesSystem.PrivilegesSet.DEFAULT.Value
                && WinttOS.UsersManager.CurrentUser.UserAccess.Value >= Users.User.AccessLevel.Default.Value)
            {
                try
                {
                    contents = File.ReadAllText(path);
                    return true;
                }
                catch
                {
                    contents = null;
                    return false;
                }
            }

            contents = null;
            return false;
        }
        public static bool TryWriteFile(Process process, string path, string contents)
        {
            if (process.CurrentSet.Value >= PrivilegesSystem.PrivilegesSet.DEFAULT.Value
                && WinttOS.UsersManager.CurrentUser.UserAccess.Value >= Users.User.AccessLevel.Default.Value)
            {
                if (ReadonlyFiles.Contains(path))
                    return false;

                try
                {
                    File.WriteAllText(path, contents);
                }
                catch
                {
                    return false;
                }

            }

            return false;
        }
    }
}
