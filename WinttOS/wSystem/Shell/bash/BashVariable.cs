namespace WinttOS.wSystem.Shell.bash
{
    public sealed class BashVariable
    {
        public string Name { get; }
        public string Value { get; set; }

        public BashVariable(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
