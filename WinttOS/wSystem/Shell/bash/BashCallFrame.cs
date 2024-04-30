namespace WinttOS.wSystem.Shell.bash
{
    public sealed class BashCallFrame
    {
        public readonly int CallLine;
        public readonly BashFunction Function;

        public BashCallFrame(int callLine, BashFunction function)
        {
            CallLine = callLine;
            Function = function;
        }
    }
}
