namespace WinttOS.wSystem.Registry
{
    public struct EnvKey
    {
        public string Name;
        public string Value;

        public EnvKey(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
