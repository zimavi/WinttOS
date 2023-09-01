﻿using System;
using System.IO;
using System.Text;
using System.Linq;
using WinttOS.Base.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WinttOS.Base
{
    public class GlobalData
    {
        public static string currDir = "";
        public static Cosmos.System.FileSystem.CosmosVFS fs;
        public static string fileToEdit;
        public static UI ui;
    }
}
