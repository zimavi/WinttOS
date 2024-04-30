using WinttOS.Core.Utils.Sys;

namespace WinttOS.Core.Utils.Kernel
{
    public sealed class WinttStatus : SmartEnum<WinttStatus, uint>
    {

        // Crash codes

        public static readonly WinttStatus CRITICAL_PROCESS_DIED                = new("CRITICAL_PROCESS_DIED", 0x00000001);
        public static readonly WinttStatus SYSTEM_THREAD_EXCEPTION_NOT_HANDLED  = new("SYSTEM_THREAD_EXCEPTION_NOT_HANDLED", 0x00000002);
        public static readonly WinttStatus SYSTEM_SERVICE_EXCEPTION             = new("SYSTEM_SERVICE_EXCEPTION", 0x00000003);
        public static readonly WinttStatus INVALID_PROCESS_ATTACH_ATTEMPT       = new("INVALID_PROCESS_ATTACH_ATTEMPT", 0x00000004);
        public static readonly WinttStatus INVALID_PROCESS_DETACH_ATTEMPT       = new("INVALID_PROCESS_DETACH_ATTEMPT", 0x00000005);
        public static readonly WinttStatus TRAP_CAUSE_UNKNOWN                   = new("TRAP_CAUSE_UNKNOWN", 0x00000006);
        public static readonly WinttStatus MEMORY_MANAGEMENT                    = new("MEMORY_MANAGEMENT", 0x00000007);
        public static readonly WinttStatus FILE_SYSTEM                          = new("FILE_SYSTEM", 0x00000008);
        public static readonly WinttStatus SECURITY_SYSTEM                      = new("SECURITY_SYSTEM", 0x00000009);
        public static readonly WinttStatus PHASE0_INITIALIZATION_FAILED         = new("PHASE0_INITIALIZATION_FAILED", 0x0000000A);
        public static readonly WinttStatus PHASE1_INITIALIZATION_FAILED         = new("PHASE1_INITIALIZATION_FAILED", 0x0000000B);
        public static readonly WinttStatus CRITICAL_SERVICE_FAILED              = new("CRITICAL_SERVICE_FAILED", 0x0000000C);
        public static readonly WinttStatus SET_ENV_VAR_FAILED                   = new("SET_ENV_VAR_FAILED", 0x0000000D);
        public static readonly WinttStatus PROCESS_INITIALIZATION_FAILED        = new("PROCESS_INITIALIZATION_FAILED", 0x0000000E);
        public static readonly WinttStatus OBJECT_INITIALIZATION_FAILED         = new("OBJECT_INITIALIZATION_FAILED", 0x0000000F);              // UNUSED
        public static readonly WinttStatus CONFIG_INITIALIZATION_FAILED         = new("CONFIG_INITIALIZATION_FAILED", 0x00000010);
        public static readonly WinttStatus CONFIG_LIST_FAILED                   = new("CONFIG_LIST_FAILED", 0x00000011);
        public static readonly WinttStatus BAD_SYSTEM_CONFIG_INFO               = new("BAD_SYSTEM_CONFIG_INFO", 0x00000012);
        public static readonly WinttStatus CANNOT_WRITE_CONFIGURATION           = new("CANNOT_WRITE_CONFIGURATION", 0x00000013);
        public static readonly WinttStatus INSTALL_MORE_MEMORY                  = new("INSTALL_MORE_MEMORY", 0x00000014);
        public static readonly WinttStatus SETUP_FAILURE                        = new("SETUP_FAILURE", 0x00000015);
        public static readonly WinttStatus VIDEO_DRIVER_INIT_FAILURE            = new("VIDEO_DRIVER_INIT_FAILURE", 0x00000016);
        public static readonly WinttStatus ATTEMPTED_WRITE_TO_READONLY_MEMORY   = new("ATTEMPTED_WRITE_TO_READONLY_MEMORY", 0x00000017);
        public static readonly WinttStatus BAD_POOL_CALLER                      = new("BAD_POOL_CALLER", 0x00000018);
        public static readonly WinttStatus MANUALLY_INITIATED_CRASH             = new("MANUALLY_INITIATED_CRASH", 0x00000019);
        public static readonly WinttStatus THREAD_STUCK                         = new("THREAD_STUCK", 0x0000001A);
        public static readonly WinttStatus STATUS_CANNOT_LOAD_REGISTRY_FILE     = new("STATUS_CANNOT_LOAD_REGISTRY_FILE0", 0x0000001B);
        public static readonly WinttStatus STATUS_SYSTEM_PROCESS_TERMINATED     = new("STATUS_SYSTEM_PROCESS_TERMINATED", 0x0000001C);
        public static readonly WinttStatus HAL_INITIALIZATION_FAILED            = new("HAL_INITIALIZATION_FAILED", 0x0000001D);
        public static readonly WinttStatus MANUALLY_INITIATED_CRASH1            = new("MANUALLY_INITIATED_CRASH1", 0xDEADDEAD);
        
        // API returns codes

        public static readonly WinttStatus STATUS_SUCCESS                       = new ("STATUS_SUCESS", 0x0000001E, false);
        public static readonly WinttStatus STATUS_FAILURE                       = new("STATUS_FAILURE", 0x0000001F, false);

        private WinttStatus(string name, uint value, bool isStopCode = true) : base(name, value)
        {
            IsStopCode = isStopCode;
        }
        public bool IsStopCode { get; }
    }
}
