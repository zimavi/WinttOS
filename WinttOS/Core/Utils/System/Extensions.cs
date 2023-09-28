using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Debugging;
using WinttOS.System.wosh.Programs;

namespace WinttOS.Core.Utils.System
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
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.System.Extensions.SubArray()",
                "T[](this T[], int, int)", "Extensions.cs", 29));
            T[] result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            WinttCallStack.RegisterReturn();
            return result;
        }

        public static List<T> SubList<T>(this List<T> list, int offset, int length)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.System.Extensions.SubList()",
                "List<T>(this List<T>, int, int)", "Extensions.cs", 39));
            List<T> result;
            result = list.GetRange(offset, length);
            WinttCallStack.RegisterReturn();
            return result;
        }    

        #endregion

        #region Generic

        public static bool IsNull<T>(this T obj) => obj is null;

        #endregion

        #region "Extesions" for static classes

        [Obsolete("This method returns wrong string (for some reason)")]
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
