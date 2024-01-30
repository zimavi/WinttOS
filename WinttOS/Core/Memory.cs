//
// ORIGINAL CODE: https://github.com/aura-systems/Aura-Operating-System/
//

using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core
{
    public class Memory
    {
        public static uint TotalMemory = CPU.GetAmountOfRAM();
        public uint FreePercentage;
        public uint UsedPercentage = (GetUsedMemory() * 100) / TotalMemory;
        public uint FreeMemory = TotalMemory - GetUsedMemory();
        private const uint DIV = 1048576;

        public Memory()
        {
            this.Monitor();
        }

        public static void GetTotalMemory()
        {
            TotalMemory = CPU.GetAmountOfRAM() + 1;
        }

        public void Monitor()
        {
            GetTotalMemory();
            FreeMemory = TotalMemory - GetUsedMemory();
            UsedPercentage = (GetUsedMemory() * 100) / TotalMemory;
            FreePercentage = 100 - UsedPercentage;
        }

        public static uint GetFreeMemory() => TotalMemory - GetUsedMemory();

        public static uint GetUsedMemory()
        {
            uint UsedRAM = CPU.GetEndOfKernel() + 1024;
            return UsedRAM / DIV;
        }
    }
}
