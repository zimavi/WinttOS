using WinttOS.Core.Utils;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;

using WinttOS.wSystem;

namespace WinttOS.Core
{
    public class GlobalData
    {
        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.zap-ext-light18.psf")]
        private static byte[] _fallbackFont;
        public static string CurrentVolume = @"0:\";
        public static string CurrentDirectory = @"0:\";
        public static Cosmos.System.FileSystem.CosmosVFS FileSystem;
        public static string FileToEdit;
        public static readonly PCScreenFont FallbackFont = PCScreenFont.LoadFont(_fallbackFont);
        public static int ShellClearStartPos
        {
            get
            {
                return 2 + CurrentDirectory.Length + wSystem.WinttOS.UsersManager.CurrentUser.Name.Length;
            }
        }
    }
}
