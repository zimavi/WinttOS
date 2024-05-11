using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.wAPI.PrivilegesSystem;

namespace WinttOS.wSystem.Users
{
    public sealed class User
    {

        public sealed class AccessLevel : SmartEnum<AccessLevel, byte>
        {
            public static readonly AccessLevel Guest = new("Guest", 0, PrivilegesSet.DEFAULT);
            public static readonly AccessLevel Default = new("Default", 1, PrivilegesSet.RAISED);
            public static readonly AccessLevel Administrator = new("Administrator", 2, PrivilegesSet.HIGHEST);

            private AccessLevel(string name, byte value, PrivilegesSet privilegeSet) : base(name, value)
            {
                this.PrivilegeSet = privilegeSet;
            }

            public readonly PrivilegesSet PrivilegeSet;
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

        public bool IsLoggedIn { get; internal set; } = false;

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
                return $@"0:\home\{Login}\";
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
            new();

        public static bool ChangePassword(ref User user, string OldPassword, string NewPassword)
        {
            if (user.PasswordHash == Sha256.hash(OldPassword) ||
                !user.HasPassword)
            {
                user.PasswordHash = Sha256.hash(NewPassword);
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if(obj.GetType() != typeof(User)) 
                return false;
            return (User)obj == this;
        }

        #endregion

        public sealed class UserBuilder
        {
            private User _user;

            public UserBuilder() => 
                _user = new();

            /// <summary>
            /// Set user's name. You must call this method to successfully build user!
            /// </summary>
            /// <param name="Name">User name string</param>
            /// <returns></returns>
            public UserBuilder SetUserName(string Name)
            {
                _user.Name = Name;
                return this;
            }

            /// <summary>
            /// Set password
            /// </summary>
            /// <param name="RawPassword">Raw password string</param>
            public UserBuilder SetPassword(string RawPassword)
            {

                _user.PasswordHash = Sha256.hash(RawPassword);

                return this;
            }

            /// <summary>
            /// Set already hashed password
            /// </summary>
            /// <param name="PasswordHash">Computed pasword hash using <see cref="MD5"/></param>
            public UserBuilder SetPasswordHash(string PasswordHash)
            {
                _user.PasswordHash = PasswordHash;
                return this;
            }

            /// <summary>
            /// Set <see cref="AccessLevel"/> for <see cref="User"/>
            /// </summary>
            /// <param name="AccessLevel">Specify new user's access level</param>
            public UserBuilder SetAccess(AccessLevel AccessLevel)
            {
                _user.UserAccess = AccessLevel;
                return this;
            }

            /// <summary>
            /// Build User
            /// </summary>
            /// <returns><see cref="User"/> object , or <see langword="null"/> if incorrect <see cref="User"/> was built</returns>
            public User? Build()
            {
                Logger.DoOSLog("[Info] Creating new user " + _user.Name);
                if (string.IsNullOrEmpty(_user.Name))
                    return null;
                return _user;
            }

            public bool TryBuild(out User user)
            {
                user = Build();
                if (user is null)
                    return false;
                return true;
            }
        }
    }

    public sealed class TempUser
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

        public sealed class TempUserBuilder
        {
            private TempUser _user;
            public TempUserBuilder() => 
                _user = new();

            public TempUserBuilder SetUserName(string Name)
            {
                _user.Name = $"$TMP.{Name}";
                return this;
            }

            public TempUserBuilder SetPasswordHash(string Password)
            {
                _user.PasswordHash = Password;
                return this;
            }

            public TempUserBuilder SetAccess(User.AccessLevel UserAccess)
            {
                _user.UserAccess = UserAccess;
                return this;
            }

            public TempUser? Build()
            {
                if (string.IsNullOrEmpty(_user.Name))
                    return null;
                return _user;
            }

            public bool TryBuild(out TempUser user)
            {
                user = Build();
                if (user is null)
                    return false;
                return true;
            }
        }
    }
}
