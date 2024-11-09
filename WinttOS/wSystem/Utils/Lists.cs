using System.Collections.Generic;

namespace WinttOS.wSystem.Utils
{
    public static class Lists
    {
        public static void CompareLists<T>(List<T> list1, List<T> list2, out List<T> uniqueToList1, out List<T> uniqueToList2)
        {
            uniqueToList1 = new List<T>();
            uniqueToList2 = new List<T>();

            foreach(var item in list1)
            {
                if(!list2.Contains(item))
                {
                    uniqueToList1.Add(item);
                }
            }

            foreach(var item in list2)
            {
                if (!list1.Contains(item))
                {
                    uniqueToList2.Add(item);
                }
            }
        }
    }
}
