using System;
using System.Collections.Generic;
using System.Linq;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.System;
using WinttOS.System.Users;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    public class UsersCommand : Command
    {
        public UsersCommand(string name) : base(name, User.AccessLevel.Administrator)
        {
            HelpCommandManager.addCommandUsageStrToManager("user [list,add,remove,change] - manipulate with users (Please read manual before using)");
        }

        public override string Execute(string[] arguments)
        {
            //return "Not implemented yet";
            //throw new NotImplementedException("Command was restricted for using as it cause system fall.");
            WinttCallStack.RegisterCall(new("WinttOS.System.wosh.commands.UsersCommand.Execute()",
                "string(string[])", "UsersCommand.cs", 18));
            WinttDebugger.Trace("Entering user's command execute function");
            if(arguments.Length == 0 || arguments[0] == "list")
            {
                WinttDebugger.Trace($"Showing list with {WinttOS.UsersManager.Users.Count} users");
                List<string> res = new();
                foreach(var user in WinttOS.UsersManager.Users)
                {
                    WinttDebugger.Trace($"Working with user '{user.Name}'");
                    if (WinttOS.UsersManager.ActiveUsers.Contains(user))
                    {
                        res.Add($"{user.Name} *");
                        continue;
                    }

                    res.Add(user.Name);
                }
                if(res.Any())
                    return string.Join('\n', res.ToArray());
                return "No users";
            }
            else if (arguments[0] == "add")
            {
                if (arguments.Length > 2)
                {
                    string Username = string.Join(' ', arguments.SubArray(1, arguments.Length));
                    Console.Write("Enter new user password: ");
                    string pass = ShellUtils.ReadLineWithInterception();
                    Console.Write("Enter new user access level\n(g - guest, d - default, a - admin):");
                    char access = Console.ReadKey().KeyChar;
                    byte accessToCreate;
                    switch(access)
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
                            return "Invalid access!";
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
            else if (arguments[0] == "remove")
            {
                if(arguments.Length > 2)
                {
                    string Username = string.Join(' ', arguments.SubArray(1, arguments.Length));
                    foreach(User user in WinttOS.UsersManager.Users)
                    {
                        if(user.Name == Username)
                        {
                            if (WinttOS.UsersManager.ActiveUsers.Contains(user))
                                return "Logout from account first!";
                            WinttOS.UsersManager.DeleteUser(user);
                            return "Done.";
                        }
                    }
                    return "No such a user!";
                }
            }
            else if (arguments[0] == "change")
            {
                if (arguments.Length > 3)
                {
                    if (arguments[2] == "--leave-from-old" || arguments[2] == "-l")
                    {
                        User user = WinttOS.UsersManager.GetUserByName(arguments[1]);
                        if (user.IsNull())
                            return "This user does not exsist!";
                        if (WinttOS.UsersManager.CurrentUser == user)
                            return "You can not login to this account again!";
                        if(user.HasPassword)
                        {
                            Console.Write("Enter password: ");
                            string pass = ShellUtils.ReadLineWithInterception();
                            WinttOS.UsersManager.LogoutFromUserAccount(WinttOS.UsersManager.CurrentUser);
                            WinttOS.UsersManager.LoginIntoUserAccount(user.Name, pass);
                        }
                        else
                        {
                            WinttOS.UsersManager.LogoutFromUserAccount(WinttOS.UsersManager.CurrentUser);
                            WinttOS.UsersManager.LoginIntoUserAccount(user.Name, null);
                        }
                    }
                    else
                    {
                        User user = WinttOS.UsersManager.GetUserByName(arguments[1]);
                        if (user.IsNull())
                            return "This user does not exsist!";
                        if (WinttOS.UsersManager.CurrentUser == user)
                            return "You can not login to this account again!";
                        if (user.HasPassword)
                        {
                            Console.Write("Enter password: ");
                            string pass = ShellUtils.ReadLineWithInterception();
                            WinttOS.UsersManager.LoginIntoUserAccount(user.Name, pass);
                        }
                        else
                        {
                            WinttOS.UsersManager.LoginIntoUserAccount(user.Name, null);
                        }
                    }
                }
            }
            return "";
        }
    }
}
