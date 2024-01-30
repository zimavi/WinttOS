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
        private List<T> _stack;

        internal List<T> ToList()
        {
            List<T> toReturn = _stack;
            toReturn.Reverse();
            return toReturn;
        }

        public Stack() => 
            _stack = new();

        public int Count => _stack.Count;

        public void Push(T item) => 
            _stack.Add(item);

        public T Peek() => 
            _stack.Last();

        public T Pop()
        {
            T popedValue = _stack.Last();
            _stack = _stack.GetRange(0, _stack.Count - 1);
            return popedValue;
        }
    }
}
