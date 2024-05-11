using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.Networking;

namespace WinttOS.wSystem.Processing
{
    public sealed class Package
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Link { get; set; }
        public string Version { get; set; }
        public bool Installed { get; set; }
        public Executable Executable { get; set; } = null;

        public void Download()
        {

            byte[] executable = Http.DownloadRawFile(Link);
            if (executable == null)
                return;
            Executable = new Executable(executable);
        }
    }
}
