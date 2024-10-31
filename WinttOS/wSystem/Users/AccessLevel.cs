using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.wAPI.PrivilegesSystem;

namespace WinttOS.wSystem.Users
{
    public sealed class AccessLevel : SmartEnum<AccessLevel, byte>
    {
        public static readonly AccessLevel Default = new("default", 0, PrivilegesSet.DEFAULT);
        public static readonly AccessLevel Administrator = new("admin", 1, PrivilegesSet.RAISED);
        public static readonly AccessLevel SuperUser = new("superuser", 2, PrivilegesSet.HIGHEST);

        private AccessLevel(string name, byte value, PrivilegesSet privilegeSet) : base(name, value)
        {
            this.PrivilegeSet = privilegeSet;
        }

        public readonly PrivilegesSet PrivilegeSet;
    }
}
