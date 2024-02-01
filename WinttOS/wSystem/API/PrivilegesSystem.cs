using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.API
{
    public static class PrivilegesSystem
    {
        [Flags]
        public enum Privileges
        {
            NONE = 0,
            FILE_READ = 1,
            FILE_WRITE = 2,
            SYSTEM_MODIFY = 4,
            USERS_CREATE = 8,
            START_NEW_PROCESS = 16,
        }
        public sealed class PrivilegesSet : SmartEnum<PrivilegesSet, byte>
        {
            public static readonly PrivilegesSet NONE
                = new ("NONE", 0, Privileges.NONE);
            public static readonly PrivilegesSet DEFAULT 
                = new ("DEFAULT", 1, (Privileges.FILE_READ | Privileges.FILE_WRITE));
            public static readonly PrivilegesSet RAISED 
                = new ("RAISED", 2, (Privileges.FILE_READ | Privileges.FILE_WRITE | Privileges.SYSTEM_MODIFY | Privileges.START_NEW_PROCESS));
            public static readonly PrivilegesSet HIGHEST 
                = new ("HIGHEST", 3, (Privileges.FILE_READ | Privileges.FILE_WRITE | Privileges.SYSTEM_MODIFY | Privileges.USERS_CREATE | Privileges.START_NEW_PROCESS));

            private PrivilegesSet(string name, byte value, Privileges privileges) : base(name, value)
            {
                this.Privileges = privileges;
            }

            public readonly Privileges Privileges;
        }
    }
}
