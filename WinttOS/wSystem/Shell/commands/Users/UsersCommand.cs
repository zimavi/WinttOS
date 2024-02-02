using System;
using System.Collections.Generic;
using System.Linq;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Users
{
    public class UsersCommand : Command
    {
        public UsersCommand(string[] name) : base(name, User.AccessLevel.Administrator)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Shell._commands.UsersCommand.Execute()",
                "string(string[])", "UsersCommand.cs", 18));

            WinttDebugger.Trace("Entering _user's command execute function");

            if (arguments[0] == "--list")
            {
                WinttDebugger.Trace($"Showing list with {WinttOS.UsersManager.Users.Count} users");
                List<string> res = new();
                foreach (var user in WinttOS.UsersManager.Users)
                {
                    WinttDebugger.Trace($"Working with _user '{user.Name}'");
                    if (WinttOS.UsersManager.ActiveUsers.Contains(user))
                    {
                        res.Add($"{user.Name} *");
                        continue;
                    }

                    res.Add(user.Name);
                }
                if (res.Any())
                    Console.WriteLine(string.Join('\n', res.ToArray()));
                Console.WriteLine("No users");
                return new(this, ReturnCode.OK);
            }
            else if (arguments[0] == "--add")
            {
                return new(this, ReturnCode.CRASH, "NotImplementedYet!");

                if (arguments.Count > 1)
                {
                    string Username = arguments[1];
                    Console.Write("Enter new _user password: ");
                    string pass = Console.ReadLine();
                    Console.Write("Enter new _user access level\n(g - guest, d - default, a - admin):");
                    char access = Console.ReadKey().KeyChar;
                    byte accessToCreate;
                    switch (access)
                    {
                        case 'G':
                        case 'g':
                            accessToCreate = 0;
                            break;
                        case 'D':
                        case 'd':
                            accessToCreate = 1;
                            break;
                        case 'A':
                        case 'a':
                            accessToCreate = 2;
                            break;
                        default:
                            return new(this, ReturnCode.ERROR_ARG, "Invalid access!");
                    }
                    if (string.IsNullOrEmpty(pass) || string.IsNullOrWhiteSpace(pass))
                    {
                        WinttOS.UsersManager.AddUser(new User.UserBuilder().SetUserName(Username)
                                                                           .SetAccess(User.AccessLevel.FromValue(accessToCreate))
                                                                           .Build());
                    }
                    else
                        WinttOS.UsersManager.AddUser(new User.UserBuilder().SetUserName(Username)
                                                                           .SetAccess(User.AccessLevel.FromValue(accessToCreate))
                                                                           .SetPassword(pass)
                                                                           .Build());
                }
            }
            else if (arguments[0] == "--remove" || arguments[0] == "-rm")
            {
                return new(this, ReturnCode.CRASH, "NotImplementedYet!");

                if (arguments.Count > 1)
                {
                    string Username = arguments[1];
                    foreach (User user in WinttOS.UsersManager.Users)
                    {
                        if (user.Name == Username)
                        {
                            if (WinttOS.UsersManager.ActiveUsers.Contains(user))
                                return new(this, ReturnCode.ERROR, "Logout from account first!");
                            WinttOS.UsersManager.DeleteUser(user);
                            Console.WriteLine("Done.");
                            return new(this, ReturnCode.OK);
                        }
                    }
                    return new(this, ReturnCode.ERROR, "No such a user!");
                }
            }
            else if (arguments[0] == "--change" || arguments[0] == "-ch")
            {
                return new(this, ReturnCode.CRASH, "NotImplementedYet!");

                if (arguments.Count > 1)
                {
                    if (arguments.Count > 2 && (arguments[2] == "--leave-from-old" || arguments[2] == "-l"))
                    {
                        User user = WinttOS.UsersManager.GetUserByName(arguments[1]);
                        if (user.IsNull())
                            return new(this, ReturnCode.ERROR, "This user does not exsist!");
                        if (WinttOS.UsersManager.CurrentUser == user)
                            return new(this, ReturnCode.ERROR, "You can not login to this account again!");
                        if (user.HasPassword)
                        {
                            Console.Write("Enter password: ");
                            string pass = Console.ReadLine();
                            if (WinttOS.UsersManager.LoginIntoUserAccount(user.Name, pass))
                                WinttOS.UsersManager.LogoutFromUserAccount(WinttOS.UsersManager.CurrentUser);
                        }
                        else
                        {
                            WinttOS.UsersManager.LoginIntoUserAccount(user.Name, null);
                            WinttOS.UsersManager.LogoutFromUserAccount(WinttOS.UsersManager.CurrentUser);
                        }
                    }
                    else
                    {
                        User user = WinttOS.UsersManager.GetUserByName(arguments[1]);
                        if (user.IsNull())
                            return new(this, ReturnCode.ERROR, "This user does not exsist!");
                        if (WinttOS.UsersManager.CurrentUser == user)
                            return new(this, ReturnCode.ERROR, "You can not login to this account again!");
                        if (user.HasPassword)
                        {
                            Console.Write("Enter password: ");
                            string pass = Console.ReadLine();
                            WinttOS.UsersManager.LoginIntoUserAccount(user.Name, pass);
                        }
                        else
                        {
                            WinttOS.UsersManager.LoginIntoUserAccount(user.Name, null);
                        }
                    }
                }
            }
            else if (arguments[0] == "--update-password" || arguments[0] == "-udps")
            {
                if (arguments.Count > 3)
                {
                    for (int i = 0; i < WinttOS.UsersManager.Users.Count; i++)
                    {
                        if (WinttOS.UsersManager.Users[i].Name.Equals(arguments[1]))
                        {
                            User u = WinttOS.UsersManager.Users[i];
                            bool result = WinttOS.UsersManager.Users[i].ChangePassword(
                                ref u, arguments[2], arguments[3]);
                            WinttOS.UsersManager.Users[i] = u;

                            WinttCallStack.RegisterReturn();

                            return new(this, result ? ReturnCode.OK : ReturnCode.ERROR,
                                result ? "Done." : "Invalid password!");
                        }
                    }
                    WinttCallStack.RegisterReturn();
                    return new(this, ReturnCode.ERROR, "Invalid user");
                }
                else
                {
                    PrintHelp();
                }
            }
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("- user {--list|--add|--remove|--change|--update-password}");
        }
    }
}
