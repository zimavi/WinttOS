﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.System
{
    public class RuntimeUtils
    {
        [Obsolete("", true)]
        public static void CodeDelay(int milliseconds)
        {
            for (int i = 0; i < milliseconds * 100000; i++)
            {
                ;
                ;
                ;
                ;
                ;
            }
        }
    }
}
