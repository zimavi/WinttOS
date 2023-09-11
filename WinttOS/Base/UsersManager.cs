using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base
{
    public class UsersManager
    {
        public enum UserAccess
        {
            Default,
            Admin
        }

        public static string currentUsersName { get; private set; }
        public static string currentUsersNameFormated 
        { 
            get
            {
                return string.Join('_', currentUsersName.Split(' ')).ToLower();
            }
        }
        public static UserAccess currentUsersAccess { get; private set; }
    }
}
