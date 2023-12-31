﻿using WinttOS.Core.Utils;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;

using WinttOS.System;

namespace WinttOS.Core
{
    public class GlobalData
    {
        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.zap-ext-light18.psf")]
        private static byte[] fallbackFont;
        public static string CurrentDirectory = "";
        public static Cosmos.System.FileSystem.CosmosVFS FileSystem;
        public static string FileToEdit;
        public static UI UI;
        public static readonly PCScreenFont FallbackFont = PCScreenFont.LoadFont(fallbackFont);
        public static int ShellClearStartPos
        {
            get
            {
                return 6 + CurrentDirectory.Length + System.WinttOS.UsersManager.CurrentUser.Name.Length;
            }
        }
    }
}
