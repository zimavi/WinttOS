using System.Collections.Generic;
using WinttOS.wSystem.Processing;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.wAPI
{
    public static class IO
    {

        public static List<string> ReadonlyFiles { get; internal set; } = new() { };
        public static bool TryMarkReadonly(Process process, string path) 
        {
            if (process.CurrentSet.Value >= PrivilegesSystem.PrivilegesSet.DEFAULT.Value 
                && UsersManager.LoggedLevel.Value >= AccessLevel.Default.Value)
            {
                ReadonlyFiles.Add(path);
                return true;
            }
            return false;
        }
    }
}
