using WinttOS.Core.Utils.Sys;

namespace WinttOS.wSystem.wAPI.PrivilegesSystem
{
    public sealed class PrivilegesSet : SmartEnum<PrivilegesSet, byte>
    {
        public static readonly PrivilegesSet NONE
            = new("NONE", 0, Privileges.NONE);
        public static readonly PrivilegesSet DEFAULT
            = new("DEFAULT", 1, Privileges.FILE_READ | Privileges.FILE_WRITE | Privileges.START_NEW_PROCESS);
        public static readonly PrivilegesSet RAISED
            = new("RAISED", 2, Privileges.FILE_READ | Privileges.FILE_WRITE | Privileges.SYSTEM_MODIFY | Privileges.START_NEW_PROCESS);
        public static readonly PrivilegesSet HIGHEST
            = new("HIGHEST", 3, Privileges.FILE_READ | Privileges.FILE_WRITE | Privileges.SYSTEM_MODIFY | Privileges.USERS_CREATE | Privileges.START_NEW_PROCESS);

        private PrivilegesSet(string name, byte value, Privileges privileges) : base(name, value)
        {
            Privileges = privileges;
        }

        public readonly Privileges Privileges;

        public static bool HasFlag(PrivilegesSet set, Privileges flag)
        {
            if (((Privileges)set.Value & flag) != 0)
                return true;
            return false;
        }
    }
}
