using System;
using System.IO;
using System.Text;
using System.Linq;
using WinttOS.Base.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;

namespace WinttOS.Base
{
    public class GlobalData
    {
        [ManifestResourceStream(ResourceName = "WinttOS.Base.resources.zap-ext-light18.psf")]
        private static byte[] _fallbakcFont;
        public static string currDir = "";
        public static Cosmos.System.FileSystem.CosmosVFS fs;
        public static string fileToEdit;
        public static UI ui;
        public static readonly PCScreenFont FallbackFont = PCScreenFont.LoadFont(_fallbakcFont);
        public static int ShellClearStartPos
        {
            get
            {
                return 5 + currDir.Length;
            }
        }
    }
}
