using System.Collections.Generic;
using WinttOS.Core.Utils.System;

namespace WinttOS.Core.Utils.Debugging
{
    public static class WinttCallStack
    {
        private static System.Stack<MethodCallInfo> callStack = new();

        public static System.Stack<MethodCallInfo> CallStack => callStack;

        public static void RegisterCall(MethodCallInfo info)
        {
            callStack.Push(info);
        }

        public static void RegisterReturn() => callStack.Pop();

        internal static string GetCallStack()
        {
            MethodCallInfo lastCall = callStack.Pop();
            List<string> toReturn = new()
            {
                $"Last call: {lastCall.MethodFullPath} in {lastCall.FileName}:{lastCall.FileLineNumber}"
            };
            foreach (MethodCallInfo info in callStack.ToList())
            {
                toReturn.Add($"  at {info.MethodFullPath} in {info.FileName}:{info.FileLineNumber}");
            }
            return string.Join("\n----------\n", toReturn.ToArray());
        }
    }
}
