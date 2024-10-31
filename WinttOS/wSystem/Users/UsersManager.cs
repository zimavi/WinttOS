using System.Collections.Generic;
using System.IO;
using System.Text;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Serialization;

namespace WinttOS.wSystem.Users
{
    public sealed class UsersManager
    {
        #region User Directories
        public static void InitUserDirs(string user)
        {
            if (user == "root")
                return;

            string[] defaultDirs =
            {
                @"0:\home\" + user + @"\Desktop",
                @"0:\home\" + user + @"\Documents",
                @"0:\home\" + user + @"\Downloads",
                @"0:\home\" + user + @"\Music",
            };
            foreach (string dir in defaultDirs)
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }
        #endregion

        public static string[] users;
        private static string[] reset;
        private static List<string> userfile = new();
        public static AccessLevel LoggedLevel;
        public static bool loggedIn = false;
        public static string userLogged;
        public static string userDir;

        #region Methods

        public void Create(string username, string password, string type = "default")
        {
            try
            {
                password = Sha256.hash(password);
                LoadUsers();
                if(GetUser("user").StartsWith(username))
                {
                    SystemIO.STDOUT.PutLine(username + " already exists");
                    return;
                }
                PutUser("user:" + username, password + "|" + type);
                PushUsers();
                SystemIO.STDOUT.PutLine(username + " has been created");

                InitUserDirs(username);
                SystemIO.STDOUT.PutLine("Home directories has been created");
            }
            catch
            {
                SystemIO.STDOUT.PutLine("Unable to create user");
            }
        }

        public void Remove(string username)
        {
            if (GetUser("user").StartsWith(username))
            {
                LoadUsers();
                DeleteUser(username);
                SystemIO.STDOUT.PutLine(username + " has been deleted");
            }
            else
            {
                SystemIO.STDOUT.PutLine(username + " does not exists");
            }
        }

        public void ChangePassword(string username, string password)
        {
            LoadUsers();
            EditUser(username, password, LoggedLevel.Name);
            File.Delete(@"0:\etc\passwd");
            File.Create(@"0:\etc\passwd");
            PushUsers();
            SystemIO.STDOUT.PutLine("Password has been changed");
        }

        #endregion

        #region Static Methods

        public static void DeleteUser(string username)
        {
            foreach(string line in users)
            {
                userfile.Add(line);
            }

            int counter = -1;
            int idx = 0;

            bool exists = false;

            foreach(string str in userfile)
            {
                counter++;
                if(str.Contains(username))
                {
                    idx = counter;
                    exists = true;
                    break;
                }
            }
            if (exists)
            {
                userfile.RemoveAt(idx);

                users = userfile.ToArray();

                userfile.Clear();

                File.Delete(@"0:\etc\passwd");

                PushUsers();
            }
        }

        public static void EditUser(string username, string password, string type)
        {
            password = Sha256.hash(password);
            EditUserHashed(username, password, type);
        }

        public static void EditUserHashed(string username, string password, string type)
        {
            foreach (string line in users)
            {
                userfile.Add(line);
            }

            int counter = -1;
            int idx = 0;

            bool exists = false;

            foreach (string element in userfile)
            {
                counter++;
                if (element.Contains(username))
                {
                    idx = counter;
                    exists = true;
                    break;
                }
            }
            if (exists)
            {
                userfile[idx] = "user:" + username + ":" + password + "|" + type;

                users = userfile.ToArray();

                userfile.Clear();
            }
        }

        public static string GetUser(string parameter)
        {
            string value = "null";

            foreach (string line in users)
            {
                userfile.Add(line);
            }

            foreach (string element in userfile)
            {
                if (element.StartsWith(parameter))
                {
                    value = element.Remove(0, parameter.Length + 1);
                }
            }

            userfile.Clear();

            return value;
        }

        public static void PutUser(string parameter, string value)
        {
            bool contains = false;

            foreach (string line in users)
            {
                userfile.Add(line);
                if (line.StartsWith(parameter))
                {
                    contains = true;
                    break;
                }
            }

            if (!contains)
            {
                userfile.Add(parameter + ":" + value);
            }

            users = userfile.ToArray();

            userfile.Clear();
        }

        public static void PushUsers()
        {
            File.WriteAllLines(@"0:\etc\passwd", users);
        }

        public static void LoadUsers()
        {
            users = reset;

            if (!Directory.Exists(@"0:\etc"))
                Directory.CreateDirectory(@"0:\etc");

            try
            {
                users = File.ReadAllLines(@"0:\etc\passwd");
            } 
            catch (FileNotFoundException)
            {
                PutUser("user:" + "root", Sha256.hash("root") + "|" + "superuser");
                PushUsers();
            }
        }

        #endregion
    }
}
