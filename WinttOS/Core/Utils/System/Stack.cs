using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.System
{
    public class Stack<T>
    {
        private List<T> stack;

        internal List<T> ToList()
        {
            List<T> toReturn = stack;
            toReturn.Reverse();
            return toReturn;
        }

        public Stack()
        {
            stack = new();
        }

        public int Count => stack.Count;

        public void Push(T item) => stack.Add(item);

        public T Peek() => stack.Last();

        public T Pop()
        {
            T popedValue = stack.Last();
            stack = stack.GetRange(0, stack.Count - 1);
            return popedValue;
        }
    }
}
