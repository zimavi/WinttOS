using System;

namespace WinttOS.wSystem.wAPI.PrivilegesSystem
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
}
