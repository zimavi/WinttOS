using System;
using WinttOS.wSystem.Users;

namespace WinttOS.Core
{
    public sealed class GlobalData
    {
        public static string CurrentVolume = @"0:\";
        public static string CurrentDirectory = @"0:\";
        public static Cosmos.System.FileSystem.CosmosVFS FileSystem;
        public static string FileToEdit;

        public static readonly string TREE_PART1 = new(new char[] { (char)0x0c4, (char)0x0c0, (char)0x0c0, ' ' }); //"└── "
        public static readonly string TREE_PART2 = new(new char[] { (char)0x0c1, ' ', ' ', ' ' });                 //"│   "
        public static readonly string TREE_PART3 = new(new char[] { (char)0x0c6, (char)0x0c0, (char)0x0c0, ' ' }); //"├── "
        public static int ShellClearStartPos
        {
            get
            {
                return 3 + CurrentDirectory.Length + UsersManager.userLogged.Length;
            }
        }

        public static ulong EPOCH
        {
            get
            {
                DateTime today = DateTime.Now;
                DateTime epoch = new DateTime(1970, 1, 1);

                return (ulong)(today - epoch).TotalSeconds;
            }
        }
    }
}
