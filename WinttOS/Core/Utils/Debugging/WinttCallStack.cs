using System.Collections.Generic;
using WinttOS.Core.Utils.System;

namespace WinttOS.Core.Utils.Debugging
{
    public static class WinttCallStack
    {
        private static System.Stack<MethodCallInfo> _callStack = new();

        public static System.Stack<MethodCallInfo> CallStack => _callStack;

        public static void RegisterCall(MethodCallInfo info)
        {
            _callStack.Push(info);
        }

        public static void RegisterReturn() => _callStack.Pop();

        internal static string GetCallStack()
        {
            MethodCallInfo lastCall = _callStack.Peek();
            List<string> toReturn = new()
            {
                $"Last call: {lastCall.MethodFullPath} in {lastCall.FileName}:{lastCall.FileLineNumber}"
            };
            foreach (MethodCallInfo info in _callStack.ToList())
            {
                if (info == lastCall)
                    continue;
                toReturn.Add($"  at {info.MethodFullPath} in {info.FileName}:{info.FileLineNumber}");
            }
            return string.Join("\n----------\n", toReturn.ToArray());
        }
    }
}
