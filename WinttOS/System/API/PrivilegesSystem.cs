using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.System;
using WinttOS.System.Users;

namespace WinttOS.System.API
{
    public static class PrivilegesSystem
    {
        [Flags]
        public enum Privileges
        {
            None = 0,
            FILE_READ = 1,
            FILE_WRITE = 2,
            SYSTEM_MODIFY = 4,
            USERS_CREATE = 8,
        }
        public sealed class PrivilegesSet : SmartEnum<PrivilegesSet, byte>
        {
            public static readonly PrivilegesSet DEFAULT = new ("DEFAULT", 0, 
                (Privileges.FILE_READ | Privileges.FILE_WRITE));
            public static readonly PrivilegesSet RAISED = new ("RAISED", 1, 
                (Privileges.FILE_READ | Privileges.FILE_WRITE | Privileges.SYSTEM_MODIFY));
            public static readonly PrivilegesSet HIGHEST = new ("HIGHEST", 2, 
                (Privileges.FILE_READ | Privileges.FILE_WRITE | Privileges.SYSTEM_MODIFY | Privileges.USERS_CREATE));

            private PrivilegesSet(string name, byte value, Privileges privileges) : base(name, value)
            {
                this.Privileges = privileges;
            }

            public readonly Privileges Privileges;
        }
    }
}
