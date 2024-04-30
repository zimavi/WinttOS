using System;

namespace WinttOS.wSystem.Services
{
    [Flags]
    public enum ServiceStatus : byte
    {
        no_data = 0,
        OK = 1,
        OFF = 2,
        PAUSED = 4,
        PENDING = 8,
        ERROR = 16,
    }

    public static class ServiceStatusFormatter
    {
        public static string ToStringEnum(ServiceStatus status)
        {
            switch(status)
            {
                case ServiceStatus.no_data: return "no_data";
                case ServiceStatus.OK:      return "OK";
                case ServiceStatus.OFF:     return "OFF";
                case ServiceStatus.PAUSED:  return "PAUSED";
                case ServiceStatus.PENDING: return "PENDING";
                case ServiceStatus.ERROR:   return "ERROR";
                default:                    return "Not a ServiceStatus";
            }
        }
    }
}
