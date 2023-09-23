using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.System.Users
{
    public class User
    {
        public enum AccessLevel : byte
        {
            Guest,
            Default,
            Administrator
        }

        #region Fields
        public string Login { 
            get
            {
                return string.Join('_', Name.Split(' ')).ToLower();
            }
        }

        public string Name { get; private set; }

        public string PasswordHash { get; private set; }

        public bool HasPassword
        {
            get
            {
                return !string.IsNullOrEmpty(PasswordHash);
            }
        }

        public string UsersFolderLocation
        {
            get
            {
                return $@"0:\Users\{Login}\";
            }
        }

        public AccessLevel UserAccess { get; private set; }

        #endregion

        #region Constructors

        private User()
        {
            Name = "";
            PasswordHash = "";
            UserAccess = AccessLevel.Default;
        }

        #endregion

        #region Operators

        public static bool operator ==(User u1, User u2) => 
            (u1.Name.Equals(u2.Name) &&
                u1.PasswordHash.Equals(u2.PasswordHash) &&
                u1.UserAccess.Equals(u2.UserAccess));

        public static bool operator !=(User u1, User u2) => !(u1.Name.Equals(u2.Name) &&
                u1.PasswordHash.Equals(u2.PasswordHash) &&
                u1.UserAccess.Equals(u2.UserAccess));

        #endregion

        #region Methods                                                                                                                             //new(Name, Password, AccessLevel.Administrator);

        public static User CreateGuestAccount() => 
            new UserBuilder().SetUserName("Guest")
                             .SetAccess(AccessLevel.Guest)
                             .Build();
        public static User CreateEmptyUser() => 
            new User();

        public bool ChangePassword(ref User user, string OldPassword, string NewPassword)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Users.User.ChangePassword()",
                "bool(ref User, string, string)", "User.cs", 86));
            if (user.PasswordHash == MD5.Calculate(Encoding.UTF8.GetBytes(OldPassword)) ||
                !user.HasPassword)
            {
                user.PasswordHash = MD5.Calculate(Encoding.UTF8.GetBytes(NewPassword));
                WinttCallStack.RegisterReturn();
                return true;
            }
            WinttCallStack.RegisterReturn();
            return false;
        }

        public override bool Equals(object obj)
        {
            if(obj.GetType() != typeof(User)) 
                return false;
            return (User)obj == this;
        }

        #endregion

        public class UserBuilder
        {
            private User user;

            public UserBuilder() => 
                user = new();

            /// <summary>
            /// Set user's name. You <c>must</c> call this method to successfully build user!
            /// </summary>
            /// <param name="Name">User name string</param>
            /// <returns></returns>
            public UserBuilder SetUserName(string Name)
            {
                user.Name = Name;
                return this;
            }

            /// <summary>
            /// Set password
            /// </summary>
            /// <param name="RawPassword">Raw password string</param>
            public UserBuilder SetPassword(string RawPassword)
            {
                WinttCallStack.RegisterCall(new("WinttOS.System.Users.User.UserBuilder.SetPassword()",
                    "UserBuilder(string)", "User.cs", 132));

                user.PasswordHash = MD5.Calculate(Encoding.UTF8.GetBytes(RawPassword));

                WinttCallStack.RegisterReturn();
                return this;
            }

            /// <summary>
            /// Set already hashed password
            /// </summary>
            /// <param name="PasswordHash">Computed pasword hash using <see cref="MD5"/></param>
            public UserBuilder SetHashedPassword(string PasswordHash)
            {
                user.PasswordHash = PasswordHash;
                return this;
            }

            /// <summary>
            /// Set <see cref="AccessLevel"/> for <see cref="User"/>
            /// </summary>
            /// <param name="AccessLevel">Specify new user's access level</param>
            public UserBuilder SetAccess(AccessLevel AccessLevel)
            {
                user.UserAccess = AccessLevel;
                return this;
            }

            /// <summary>
            /// Build User
            /// </summary>
            /// <returns><see cref="User"/> object , or <see langword="null"/> if incorrect <see cref="User"/> was built</returns>
            public User Build()
            {
                if(string.IsNullOrEmpty(user.Name))
                    return null;
                return user;
            }
        }
    }

    public class TempUser
    {
        #region Fields
        public string Login
        {
            get
            {
                try
                {
                    return string.Join('_', Name.Split(' ')).ToLower();
                }
                catch
                {
                    return null;
                }
            }
        }

        public string Name { get; private set; } = null;

        public string? PasswordHash { get; private set; } = null;

        public bool HasPassword
        {
            get
            {
                return !string.IsNullOrEmpty(PasswordHash);
            }
        }

        public User.AccessLevel? UserAccess { get; private set; } = null;

        #endregion

        private TempUser() { }

        public class TempUserBuilder
        {
            private TempUser user;
            public TempUserBuilder() => 
                user = new();

            public TempUserBuilder SetUserName(string Name)
            {
                user.Name = $"$TMP-{Name}";
                return this;
            }

            public TempUserBuilder SetPasswordHash(string Password)
            {
                user.PasswordHash = Password;
                return this;
            }

            public TempUserBuilder SetAccess(User.AccessLevel UserAccess)
            {
                user.UserAccess = UserAccess;
                return this;
            }

            public TempUser? Build()
            {
                if (string.IsNullOrEmpty(user.Name))
                    return null;
                return user;
            }
        }
    }
}
