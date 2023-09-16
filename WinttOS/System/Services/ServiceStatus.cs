using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.Services
{
    public enum ServiceStatus : sbyte
    {
        OK,
        OFF,
        ERROR,
        no_data = -1
    }
}
