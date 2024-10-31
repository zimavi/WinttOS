using WinttOS.wSystem.Users;

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
                return 3 + CurrentDirectory.Length + UsersManager.userLogged.Length;
            }
        }
    }
}
