

using WinttOS.Core.Utils.System;

namespace WinttOS.Core.Utils.Debugging
{
    public static class WinttCallStack
    {
        private static Stack<MethodCallInfo> callStack = new();

        public static Stack<MethodCallInfo> CallStack => callStack;

        public static void RegisterCall(MethodCallInfo info)
        {
            callStack.Push(info);
        }

        public static void RegisterReturn() => callStack.Pop();

        internal static string GetCallStack()
        {
            MethodCallInfo lastCall = callStack.Pop(); 
            string toReturn = $"Last call: {lastCall.MethodFullPath} in {lastCall.FileName}:{lastCall.FileLineNumber}";
            foreach(MethodCallInfo info in callStack.ToList())
            {
                toReturn += $"  at {info.MethodFullPath} in {lastCall.FileName}:{lastCall.FileLineNumber}\n";
            }
            return toReturn;
        }
    }
}
