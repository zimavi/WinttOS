
using acryptohashnet;
using System.IO;
using System.Text;
using WinttOS.Core;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Sys;

namespace UniLua
{
    internal class LuaCosmosCryptoLib
    {
        public const string LIB_NAME = "cosmos.crypto";

        public static int OpenLib(ILuaState lua)
        {
            NameFuncPair[] define = new NameFuncPair[]
            {
                new NameFuncPair( "strtomd5",   CRYPTO_md5  ),
                new NameFuncPair( "strtosha256",    CRYPTO_sha256  ),
                new NameFuncPair( "strtosha512",    CRYPTO_sha512  ),
                new NameFuncPair("filetomd5", FILE_CRYPTO_md5),
                new NameFuncPair("filetosha256", FILE_CRYPTO_sha256),
                new NameFuncPair("filetosha512", FILE_CRYPTO_sha512),
            };

            lua.L_NewLib(define);
            return 1;
        }

        private static int CRYPTO_md5(ILuaState lua)
        {
            string input = lua.L_CheckString(1);
            var hashBytes = WinttOS.Core.Utils.Cryptography.MD5.hash(input);
            lua.PushString(hashBytes);
            return 1;
        }

        private static int CRYPTO_sha256(ILuaState lua)
        {
            string input = lua.L_CheckString(1);
            var hashBytes = Sha256.hash(Encoding.UTF8.GetBytes(input));
            lua.PushString(hashBytes);
            return 1;
        }

        private static int CRYPTO_sha512(ILuaState lua)
        {
            string input = lua.L_CheckString(1);
            var hashAlgorithm = new SHA512();
            var hashBytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            lua.PushString(ToHexString(hashBytes));
            return 1;
        }

        private static int FILE_CRYPTO_md5(ILuaState lua)
        {
            string filePath = Path.Combine(GlobalData.CurrentDirectory, lua.L_CheckString(1));
            var hashAlgorithm = new acryptohashnet.MD5();
            using (var stream = File.OpenRead(filePath))
            {
                var hashBytes = hashAlgorithm.ComputeHash(stream);
                lua.PushString(ToHexString(hashBytes));
            }
            return 1;
        }

        private static int FILE_CRYPTO_sha256(ILuaState lua)
        {
            string filePath = Path.Combine(GlobalData.CurrentDirectory, lua.L_CheckString(1));
            byte[] file = File.ReadAllBytes(filePath);
            var hashBytes = Sha256.hash(file);
            lua.PushString(hashBytes);
            return 1;
        }

        private static int FILE_CRYPTO_sha512(ILuaState lua)
        {
            string filePath = Path.Combine(GlobalData.CurrentDirectory, lua.L_CheckString(1));
            var hashAlgorithm = new SHA512();
            using (var stream = File.OpenRead(filePath))
            {
                var hashBytes = hashAlgorithm.ComputeHash(stream);
                lua.PushString(ToHexString(hashBytes));
            }
            return 1;
        }

        private static string ToHexString(byte[] bytes)
        {
            return WinttOS.wSystem.Shell.Utils.Conversion.Hex(bytes);
        }
    }
}

