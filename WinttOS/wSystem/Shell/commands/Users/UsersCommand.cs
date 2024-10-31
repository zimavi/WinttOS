using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WinttOS.Core;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Shell.Utils;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Users
{
    public sealed class UsersCommand : Command
    {
        public UsersCommand(string[] name) : base(name, AccessLevel.Default)
        {
            CommandManual = new List<string>()
            {
                "NAME",
                "   user - Manage user accounts and user permissions",
                "",
                "SYNOPSIS",
                "   user [OPTION] [ARGS...]",
                "",
                "DESCRIPTION",
                "   The user command provides options to manage user accounts including",
                "   listing, adding, removing, updating, and switching users.",
                "",
                "OPTIONS",
                "   -l, --list",
                "       List all users along with their account types.",
                "",
                "   -a, --add [USERNAME]",
                "       Add a new user. If USERNAME is not provided you will be prompted",
                "       to enter it.",
                "",
                "   -r, --remove [USERNAME]",
                "       Remove and existing user. If USERNAME is not provided you will be",
                "       prompted to enter it.",
                "",
                "   -u, --update USERNAME [--pass NEW_PASSWORD | --type NEW_TYPE]",
                "       Update the password or type of an existing user.",
                "",
                "       --pass, -p",
                "           Update the user's password.",
                "",
                "       --type, -t",
                "           Update the user's account type. Valid types are 'default', 'admin',",
                "           and 'superuser'.",
                "",
                "   -sw, --switch USERNAME",
                "       Switch the current user to USERNAME.",
                "",
                "EXAMPLES",
                "",
                "   List all users:",
                "       user --list",
                "",
                "   Add a new user:",
                "       user --add johndoe",
                "",
                "   Remove a user:",
                "       user --remove johndoe",
                "",
                "   Update a user's password:",
                "       user --update johndoe --pass newpassword",
                "",
                "   Update a user's type:",
                "       user --update johndoe --type admin",
                "",
                "   Switch to a different user:",
                "       user --switch johndoe",
                "",
                "NOTES",
                "   - Only users with the appropriate access level can perform certain actions:",
                "     - Adding and removing users requires Administrator level access.",
                "     - Creating superusers or updating user information requires SuperUser",
                "       level access.",
                "   - The command ensures security by promting for passwords when necessary",
                "     and validating access levels.",
                "",
                "AUTHOR",
                "   ZImaVI. Developed as part of the WinttOS user management module."
            };
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "--list" || arguments[0] == "-l")
            {
                ConsoleColumnFormatter formatter = new ConsoleColumnFormatter(20, 2);
                formatter.Write("Username");
                formatter.Write("Type");
                foreach (string user in UsersManager.users)
                {
                    formatter.Write(user.Split(':')[1]);
                    formatter.Write(user.Split('|')[1]);
                }
            }
            else if (arguments[0] == "--add" || arguments[0] == "-a")
            {
                if (UsersManager.LoggedLevel.Value < AccessLevel.Administrator.Value)
                {
                    return new(this, ReturnCode.ERROR, "Access denied");
                }

                string username;
                if (arguments.Count >= 2)    // cmd --add username
                    username = arguments[1];
                else
                {
                    SystemIO.STDOUT.Put("Enter username: ");
                    username = SystemIO.STDIN.Get();
                }

                SystemIO.STDOUT.PutLine("Enter password:");
                string password = SystemIO.STDIN.Get(true);

                SystemIO.STDOUT.Put("Enter account type (d - default, a - admin, u - superuser): ");
                char type = SystemIO.STDIN.GetChr().KeyChar.ToLower();
                string level;
                Logger.DoOSLog("Checking new user type");
                switch (type)
                {
                    case 'd':
                        level = "default";
                        Logger.DoOSLog("Type: default");
                        break;

                    case 'a':
                        level = "admin";
                        Logger.DoOSLog("Type: admin");
                        break;

                    case 'u':
                        if (UsersManager.LoggedLevel.Value < AccessLevel.SuperUser.Value)
                            return new(this, ReturnCode.ERROR, "Access denied (Cannot create superuser)");
                        level = "superuser";
                        Logger.DoOSLog("Type: superuser");
                        break;

                    default:
                        return new(this, ReturnCode.ERROR, "Invalid account type.");
                }

                Logger.DoOSLog("Creating user");
                WinttOS.UsersManager.Create(username, password, level);
                SystemIO.STDOUT.PutLine("User '" + username + "' created successfully");
            }
            else if (arguments[0] == "--remove" || arguments[0] == "-r")
            {
                if (UsersManager.LoggedLevel.Value < AccessLevel.Administrator.Value)
                {
                    return new(this, ReturnCode.ERROR, "Access denied");
                }

                string username;
                if (arguments.Count > 2)    // cmd --remove username
                    username = arguments[1];
                else
                {
                    SystemIO.STDOUT.Put("Enter username: ");
                    username = SystemIO.STDIN.Get();
                }

                if (username == UsersManager.userLogged)
                    return new(this, ReturnCode.ERROR, "Cannot delete current user");

                if (username == "root")
                    return new(this, ReturnCode.ERROR, "Cannot delete 'root'");

                if (UsersManager.GetUser("user:" + username).Contains("superuser"))
                {
                    if (UsersManager.LoggedLevel.Value < AccessLevel.SuperUser.Value)
                    {
                        return new(this, ReturnCode.ERROR, "Access denied");
                    }
                }

                WinttOS.UsersManager.Remove(username);
            }
            else if (arguments[0] == "--update" || arguments[0] == "-u")
            {
                if (UsersManager.LoggedLevel.Value < AccessLevel.SuperUser.Value)
                    return new(this, ReturnCode.ERROR, "Access denied");

                if (arguments.Count < 4) // cmd --update user [--pass|--type] value
                    return new(this, ReturnCode.ERROR_ARG);

                string username = arguments[1];

                if (arguments[2] == "--pass" || arguments[2] == "-p")
                {
                    string password = arguments[3];

                    WinttOS.UsersManager.ChangePassword(username, password);
                }
                else if (arguments[2] == "--type" || arguments[2] == "-t")
                {
                    string type;
                    switch (arguments[3])
                    {
                        case "default":
                            type = "default";
                            break;
                        case "admin":
                            type = "admin";
                            break;
                        case "superuser":
                            type = "superuser";
                            break;
                        default:
                            return new(this, ReturnCode.ERROR_ARG);
                    }

                    UsersManager.EditUserHashed(username,
                        UsersManager.GetUser("user:" + username).Split(':')[2].Split('|')[0], // user:username:password|type
                        type);
                }
                else
                    return new(this, ReturnCode.ERROR_ARG);
            }
            else if (arguments[0] == "--switch" || arguments[0] == "-sw")
            {
                if (arguments.Count < 2) // cmd --switch username
                    return new(this, ReturnCode.ERROR_ARG);

                string user = UsersManager.GetUser("user:" + arguments[1]);
                if (user == "null")
                    return new(this, ReturnCode.ERROR, "This user does not exist");

                SystemIO.STDOUT.PutLine("Enter password:");
                string password = Sha256.hash(SystemIO.STDIN.Get(true));

                if (user.Contains(password))
                {
                    if (arguments[1] != "root")
                    {
                        UsersManager.userDir = @"0:\home\" + arguments[1] + @"\";
                        GlobalData.CurrentDirectory = UsersManager.userDir;
                        UsersManager.userLogged = arguments[1];
                        UsersManager.LoggedLevel = AccessLevel.FromName(user.Split('|')[1]);
                    }
                    else
                    {
                        UsersManager.userDir = @"0:\root\";
                        GlobalData.CurrentDirectory = @"0:\";
                        UsersManager.userLogged = "root";
                        UsersManager.LoggedLevel = AccessLevel.SuperUser;
                    }
                }
                else
                    return new(this, ReturnCode.ERROR, "Invalid password. Try again");
            }
            else
                return new(this, ReturnCode.ERROR_ARG);
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("user [--list] [--add] [--remove] [--update] [--switch]");
        }
    }
}
