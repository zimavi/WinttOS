using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WinttOS.Core.Utils.Debugging;
using WinttOS.System.Users;

namespace WinttOS.System.Serialization
{
    public class WinttUserSerializer
    {
        public string Serialize(User user)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Serialization.WinttUserSerializer.Serialize()",
                "string(User)", "WinttSerializer.cs", 12));
            string partialSerizlizedStr = $"(User) " +
                $"{user.Name} {user.PasswordHash} {(byte)user.UserAccess}\n";

            byte[] String2ByteArray = new byte[partialSerizlizedStr.Length];
            int i = 0;
            foreach(var ch in partialSerizlizedStr)
            {
                String2ByteArray[i++] = Convert.ToByte(ch);
            }
            string toReturn = string.Join(' ', String2ByteArray.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).ToArray());
            WinttCallStack.RegisterReturn();
            return toReturn;
        }

        public User Deserialize(string user)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Serialization.WinttUserSerializer.Deserialize()",
                "User(string)", "WinttSerializer.cs", 30));
            string[] binarySplit = user.Split(' ');
            byte[] Binary = new byte[binarySplit.Length];
            for(int i = 0; i < binarySplit.Length; ++i)
            {
                Binary[i] = Convert.ToByte(binarySplit[i], 2);
            }

            string partialSerializedStr = "";
            foreach(var i in Binary)
            {
                partialSerializedStr += Convert.ToChar(i);
            }
            binarySplit = partialSerializedStr.Split(' ');
            
            User toReturn =  new User.UserBuilder().SetUserName(binarySplit[1])
                                         .SetHashedPassword(binarySplit[2])
                                         .SetAccess((User.AccessLevel)Convert.ToByte(binarySplit[3]))
                                         .Build();

            WinttCallStack.RegisterReturn();
            return toReturn;
        }
    
        public string SerializeList(List<User> users)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Serialization.WinttUserSerializer.SerializeList()",
                "string(List<User>)", "WinttSerializer.cs", 57));
            string toReturn = "";
            foreach(var user in users)
            {
                toReturn += Serialize(user) + '\t';
            }
            WinttCallStack.RegisterReturn();
            return toReturn;
        }
        
        public List<User> DeserializeList(string users)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Serialization.WinttUserSerializer.DeserializeList()",
                "List<User>(string)", "WinttSerializer.cs", 70));
            string[] split = users.Split('\t');
            List<User> toReturn = new();
            foreach(var str in split)
            {
                toReturn.Add(Deserialize(str));
            }
            WinttCallStack.RegisterReturn();
            return toReturn;
        }
    }
}
