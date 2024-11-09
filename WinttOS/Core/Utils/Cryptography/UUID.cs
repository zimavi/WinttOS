using System;

namespace WinttOS.Core.Utils.Cryptography
{
    public static class UUID
    {
        public static byte[] GenerateUUID()
        {
            byte[] uuidBytes = new byte[16];
            Random rnd = new Random();

            rnd.NextBytes(uuidBytes);

            uuidBytes[6] = (byte)((uuidBytes[6] & 0x0F) | 0x40);

            uuidBytes[8] = (byte)((uuidBytes[8] & 0x3F) | 0x80);

            return uuidBytes;
        }

        public static string UUIDToString(byte[] bytes)
        {
            return string.Format(
                "{0:x2}{1:x2}{2:x2}{3:x2}-{4:x2}{5:x2}-{6:x2}{7:x2}-{8:x2}{9:x2}-{10:x2}{11:x2}{12:x2}{13:x2}{14:x2}{15:x2}",
                bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7], bytes[8], bytes[9],
                bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15]);
        }
    }
}
