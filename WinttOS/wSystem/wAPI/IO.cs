using System.Collections.Generic;
using WinttOS.wSystem.Processing;

namespace WinttOS.wSystem.wAPI
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
    }
}
