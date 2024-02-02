using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.Sys
{
    public class Stack<T>
    {
        private T[] _stack;
        private bool hasValues = false;

        internal List<T> ToList()
        {
            List<T> toReturn = new(_stack);
            return toReturn;
        }

        public Stack() =>
            _stack = new T[1];

        public int Count => _stack.Length;

        public void Push(T item)
        {
            if (hasValues)
            {
                T[] arr = new T[Count + 1];
                arr[0] = item;
                for (int i = 1; i <= Count; i++)
                {
                    arr[i] = _stack[i - 1];
                }
                _stack = arr;
            }
            else
            {
                _stack[0] = item;
                hasValues = true;
            }
        }

        public T Peek() =>
            _stack[0];

        public T Pop()
        {
            if(!hasValues)
            {
                return default(T);
            }
            T popedValue = _stack[0];
            if (Count == 1)
            {
                _stack = new T[1];
                hasValues = false;
            }
            else
            {
                T[] arr = new T[Count - 1];

                for (int i = 1; i < Count; i++)
                {
                    arr[i - 1] = _stack[i];
                }
                _stack = arr;
            }
            return popedValue;
        }
    }
}
