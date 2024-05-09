namespace WinttOS.wSystem.IO
{
    public static class SystemIO
    {
        public static IOut STDOUT { get; set; }
        public static IOut STDERR { get; set; }
        public static IIn STDIN { get; set; }
    }
}
