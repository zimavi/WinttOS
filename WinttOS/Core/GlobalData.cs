using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;

namespace WinttOS.Core
{
    public sealed class GlobalData
    {
        public static string CurrentVolume = @"0:\";
        public static string CurrentDirectory = @"0:\";
        public static Cosmos.System.FileSystem.CosmosVFS FileSystem;
        public static string FileToEdit;
        public static int ShellClearStartPos
        {
            get
            {
                return 2 + CurrentDirectory.Length + wSystem.WinttOS.UsersManager.CurrentUser.Name.Length;
            }
        }
    }
}
