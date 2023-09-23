using System;
using System.Collections.Generic;
using System.Linq;
using WinttOS.Core.Utils.System;
using WinttOS.System.Users;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    public class UsersCommand : Command
    {
        public UsersCommand(string name) : base(name, Users.User.AccessLevel.Administrator)
        {
            HelpCommandManager.addCommandUsageStrToManager("users [list,add,remove,change] - manipulate with users (Please read manual before using)");
        }

        public override string Execute(string[] arguments)
        {
            throw new NotImplementedException("Command was restricted for using as it couse system fall.");
            if(arguments.Length == 0 || arguments[0] == "list")
            {
                List<string> res = new();
                foreach(User user in WinttOS.UsersManager.Users)
                {
                    if(WinttOS.UsersManager.ActiveUsers.Contains(user))
                        res.Add(user.Name + " *");
                    else
                        res.Add(user.Name);
                }
                return string.Join('\n', res);
            }
            else if (arguments[0] == "add")
            {
                if (arguments.Length > 2)
                {
                    string Username = string.Join(' ', arguments.SubArray(1, arguments.Length));
                    Console.Write("Enter new user password: ");
                    string pass = ShellUtils.ReadLineWithInterception();
                    if (string.IsNullOrEmpty(pass))
                    {
                    }
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
                if (arguments.Length > 2)
                {
                    if (arguments.Contains("--leave-from-old") || arguments.Contains("-l"))
                    {
                        User user = WinttOS.UsersManager.GetUserByName(CommandName);
                        if (user.IsNull())
                            return "This user does not exsist!";
                        if (WinttOS.UsersManager.CurrentUser == user)
                            return "You can not login to this account again!";
                        if(user.HasPassword)
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }
            }
            return "";
        }
    }
}
