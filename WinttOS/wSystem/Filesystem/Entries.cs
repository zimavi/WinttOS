using System;
using System.IO;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Filesystem
{
    public sealed class Entries
    {
        public static bool ForceRemove(string fullPath)
        {
            string pFullPath = IOMapper.MapFHSToPhysical(fullPath);
            if (File.Exists(pFullPath))
            {
                File.Delete(pFullPath);
                return true;
            }
            else if (Directory.Exists(pFullPath))
            {
                Directory.Delete(pFullPath, true);
                return true;
            }
            else
            {
                SystemIO.STDOUT.PutLine(pFullPath + " does not exist!");
                return false;
            }
        }

        public static bool ForceCopy(string sourcePath, string destPath)
        {
            string pSourcePath = IOMapper.MapFHSToPhysical(sourcePath);
            string pDestPath = IOMapper.MapFHSToPhysical(destPath);
            if (File.Exists(pSourcePath))
            {
                File.Copy(pSourcePath, pDestPath, overwrite: true);
                return true;
            }
            else if (Directory.Exists(pSourcePath))
            {
                Entries.CopyDirectory(pSourcePath, pDestPath);
                return true;
            }
            else
            {
                SystemIO.STDOUT.PutLine("Source path does not exist!");
                return false;
            }
        }

        public static void CopyFile(string sourcePath, string destPath)
        {
            string pSourcePath = IOMapper.MapFHSToPhysical(sourcePath);
            string pDestPath = IOMapper.MapFHSToPhysical(destPath);
            if (File.Exists(pSourcePath))
            {
                File.Copy(pSourcePath, pDestPath, overwrite: true);
            }
        }

        public static void CopyDirectory(string sourceDir, string destDir)
        {
            string pSourceDir = IOMapper.MapFHSToPhysical(sourceDir);
            string pDestDir = IOMapper.MapFHSToPhysical(destDir);
            Directory.CreateDirectory(pDestDir);

            foreach (var file in Directory.GetFiles(pSourceDir))
            {
                string dest = Path.Combine(pDestDir, Path.GetFileName(file));
                File.Copy(file, dest);
            }

            foreach (var directory in Directory.GetDirectories(pSourceDir))
            {
                string dest = Path.Combine(pDestDir, Path.GetFileName(directory));
                CopyDirectory(directory, dest);
            }
        }
    }
}
