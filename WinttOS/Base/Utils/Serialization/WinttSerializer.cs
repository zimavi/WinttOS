using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WinttOS.Base.Users;

namespace WinttOS.Base.Utils.Serialization
{
    /*
    public class WinttSerializer
    {
        Type targetType;

        public WinttSerializer(Type targetType)
        {
            this.targetType = targetType;
            if (!targetType.IsDefined(typeof(DataContractAttribute), false))
                throw new InvalidOperationException("Requested type does not have DataContract attribute!");
        }

        public string Serialize(object graph)
        {
            var serializableProperties = targetType.GetProperties()
                .Where(p => p.IsDefined(typeof(DataMemberAttribute), false));

            string partialSerializedString = $"<{targetType.Name}>\n";
            foreach(var property in serializableProperties)
            {
                partialSerializedString += $"<{property.Name}>{property.GetValue(graph, null)}</{property.Name}>\n";
            }
            partialSerializedString += $"</{targetType.Name}>";
            int[] String2Binary = new int[partialSerializedString.Length];
            int i = 0;
            foreach(var ch in partialSerializedString)
            {
                String2Binary[i++] = Convert.ToInt32(ch.ToString());
            }

            string toReturn = "";

            foreach(int integer in String2Binary)
            {
                toReturn += $"{integer} ";
            }

            return toReturn;
        }
    }
    */
    
    public class WinttUserSerializer
    {
        public string Serialize(User user)
        {
            string partialSerizlizedStr = $"(User) " +
                $"{user.Name} {user.PasswordHash} {(byte)user.UserAccess}\n";

            byte[] String2ByteArray = new byte[partialSerizlizedStr.Length];
            int i = 0;
            foreach(var ch in partialSerizlizedStr)
            {
                String2ByteArray[i++] = Convert.ToByte(ch);
            }

            return string.Join(' ', String2ByteArray.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).ToArray());

            //return string.Join(' ', String2ByteArray);
        }

        public User Desirialize(string user)
        {
            string[] binarySplit = user.Split(' ');
            byte[] Binary = new byte[binarySplit.Length];
            for(int i = 0; i < binarySplit.Length; ++i)
            {
                Binary[i] = Convert.ToByte(binarySplit[i], 2);
            }
            //byte[] Binary = split.Select(byte.Parse).ToArray();

            string partialSerializedStr = "";
            foreach(var i in Binary)
            {
                partialSerializedStr += Convert.ToChar(i);
            }
            binarySplit = partialSerializedStr.Split(' ');
            //return new User(split[1], split[2], (User.AccessLevel)Convert.ToByte(split[3]), true);
            return new User.UserBuilder().SetUserName(binarySplit[1])
                                         .SetHashedPassword(binarySplit[2])
                                         .SetAccess((User.AccessLevel)Convert.ToByte(binarySplit[3]))
                                         .Build();
        }
    
        public string SerializeList(List<User> users)
        {
            string toReturn = "";
            foreach(var user in users)
            {
                toReturn += Serialize(user) + '\t';
            }
            return toReturn;
        }
        
        public List<User> DesirializeList(string users)
        {
            string[] split = users.Split('\t');
            List<User> toReturn = new();
            foreach(var str in split)
            {
                toReturn.Add(Desirialize(str));
            }
            return toReturn;
        }
    }
}
