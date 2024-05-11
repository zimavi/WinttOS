using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Shell.Programs;

namespace WinttOS.Core.Utils.Sys
{
    public static class Extensions
    {
        #region Arrays

        /// <summary>
        /// Get sub array of original one
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="array">Original array</param>
        /// <param name="offset">How many elemets cut from begining</param>
        /// <param name="length">How many elemets cut from ending</param>
        /// <returns>Sub array of original arry</returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="RankException"/>
        /// <exception cref="ArrayTypeMismatchException"/>
        /// <exception cref="InvalidCastException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }

        public static List<T> SubList<T>(this List<T> list, int offset, int length)
        {
            List<T> result;
            result = list.GetRange(offset, length);
            return result;
        }    

        #endregion

        #region Generic

        public static bool IsNull<T>(this T obj) => obj is null;

        #endregion

        #region String

        public static bool IsNullOrWhiteSpace(this string str)
        {
            if (str.IsNull() || str == "")
                return true;
            foreach(char c in str)
            {
                if (c != ' ' && c != '\n' && c != '\t' && c != '\r')
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region "Extesions" for static classes

        [Obsolete("This method returns wrong string (for some reason)", error: true)]
        public static string ReadLineWithInterception()
        {
            string returnStr = "";

            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Backspace)
                {
                    if (returnStr.Length > 0)
                        returnStr = returnStr.Substring(0, returnStr.Length - 1);
                }
                else if (info.Key == ConsoleKey.Enter)
                    return returnStr;
                else if (MIV.isForbiddenKey(info.Key))
                    returnStr += info.KeyChar;
            }
        }

        #endregion
    }
}
