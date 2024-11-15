using IL2CPU.API.Attribs;
using System.IO;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.wSystem.Registry;

namespace WinttOS.Core.Plugs
{
    [Plug(Target = typeof(Path))]
    public class PathImpl
    {
        public static string GetTempFileName()
        {
            string path = Path.GetTempPath();
            if (path == null)
                return null;

            string file = path + UUID.UUIDToString(UUID.GenerateUUID()) + ".tmp";
            File.Create(file);
            return file;
        }

        public static string GetTempPath()
        {
            if (Environment.HasProcessEnvValue(WinttOS.wSystem.WinttOS.ProcessManager.CurrentProcessID, "TMPDIR"))
                return Environment.GetProcessEnvValue(WinttOS.wSystem.WinttOS.ProcessManager.CurrentProcessID, "TMPDIR");
            if (Environment.HasValue("TMPDIR"))
                return Environment.GetValue("TMPDIR");
            return null;
        }
    }
}
