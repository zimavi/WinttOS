using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.wSystem.Serialization;

namespace WinttOS.wSystem.Users
{
    public class UsersManager
    {
        #region Fields

        private List<User> users;

        public List<User> Users => users;

        public User CurrentUser { get; private set; } = User.CreateEmptyUser();

        public List<User> ActiveUsers { get; private set; } = new();

        /// <summary>
        /// <see cref="User"/> if Root is correct or <see langword="null"/>
        /// </summary>
        public User? RootUser
        {
            get
            {
                User user = users[0];
                if (user.Login == "root" && user.UserAccess == User.AccessLevel.Administrator)
                    return user;
                return null;
            }
        }

        #endregion

        public UsersManager(List<User>? Users)
        {
            if (Users == null)
                this.users = new();
            else
                this.users = Users;
        }

        #region Methods

        public void AddUser(User User)
        {
            if (users.Contains(User))
                return;
            users.Add(User);
        }

        public bool DeleteUser(User User) => 
            users.Remove(User);

        public User GetUserByName(string name)
        {
            foreach (User user in Users) 
                if (user.Name == name) 
                    return user;
            return null;
        }

        public void SaveUsersData()
        {
            var serializer = new WinttUserSerializer();
            if (!Directory.Exists(@"0:\WinttOS"))
            {
                Directory.CreateDirectory(@"0:\WinttOS\");
                Directory.CreateDirectory(@"0:\WinttOS\System32\");
            }
            else if (!Directory.Exists(@"0:\WinttOS\System32"))
            {
                Directory.CreateDirectory(@"0:\WinttOS\System32");
            }
            
            File.WriteAllBytes(@"0:\WinttOS\System32\users.dat",
                Encoding.ASCII.GetBytes(serializer.SerializeList(users)));
        }

        public bool TryLoadUsersData()
        {
            try
            {
                byte[] UsersBytes = File.ReadAllBytes(@"0:\WinttOS\System32\users.dat");
                var serializer = new WinttUserSerializer();
                users = serializer.DeserializeList(Encoding.ASCII.GetString(UsersBytes));
                return true;
            }
            catch
            {
                users = new();
                return false;
            }
        }

        public bool LoginIntoUserAccount(string Login, string Password)
        {
            if (users.Count == 0)
                return false;

            foreach (User user in users)
            {
                if (!user.Name.Equals(Login))
                    return false;

                if (user.HasPassword)
                {
                    if (user.PasswordHash == MD5.Calculate(Encoding.UTF8.GetBytes(Password)))
                    {
                        CurrentUser = user;
                        foreach(User u in ActiveUsers)
                        {
                            if (u.Name.Equals(Login))
                                return true;
                        }

                        user.IsLoggedIn = true;
                        ActiveUsers.Add(user);

                        return true;
                    }
                }
                else
                {
                    CurrentUser = user;
                    if (!ActiveUsers.Contains(user))
                    {
                        user.IsLoggedIn = true;
                        ActiveUsers.Add(user);
                    }

                    return true;
                }
            }
            return false;
        }

        public void LogoutFromUserAccount(User user)
        {
            if (ActiveUsers.Count > 0)
            {
                foreach (User u in ActiveUsers)
                {
                    if (u == user)
                    {
                        ActiveUsers.Remove(u);
                        u.IsLoggedIn = false;
                        CurrentUser = User.CreateEmptyUser();
                        return;
                    }
                }
            }
        }

        public void LogoutFromUserAccount(string Login)
        {
            foreach (User user in ActiveUsers)
            {
                if (user.Name == Login)
                {
                    ActiveUsers.Remove(user);
                    return;
                }
            }
        }

        public TempUser? RequestAdminAccount(string Login, string Password)
        {
            foreach (User user in users)
            {
                if (user.Name == Login && user.UserAccess == User.AccessLevel.Administrator
                    && (!user.HasPassword
                    || user.PasswordHash == MD5.Calculate(Encoding.UTF8.GetBytes(Password))))
                {
                    return new TempUser.TempUserBuilder().SetUserName(user.Name)
                                                         .SetPasswordHash(MD5.Calculate(Encoding.UTF8.GetBytes(Password)))
                                                         .SetAccess(User.AccessLevel.Administrator)
                                                         .Build();
                }
            }

            return null;
        }

        #endregion
    }
}
