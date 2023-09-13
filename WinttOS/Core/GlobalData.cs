using System;
using System.IO;
using System.Text;
using System.Linq;
using WinttOS.Core.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;

namespace WinttOS.Core
{
    public class GlobalData
    {
        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.zap-ext-light18.psf")]
        private static byte[] _fallbackFont;
        public static string currDir = "";
        public static Cosmos.System.FileSystem.CosmosVFS fs;
        public static string fileToEdit;
        public static UI ui;
        public static readonly PCScreenFont FallbackFont = PCScreenFont.LoadFont(_fallbackFont);
        public static int ShellClearStartPos
        {
            get
            {
                return 6 + currDir.Length + Kernel.UsersManager.currentUser.Name.Length;
            }
        }
    }
}
