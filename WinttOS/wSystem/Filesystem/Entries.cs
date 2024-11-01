﻿using System;
using System.IO;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Filesystem
{
    public sealed class Entries
    {
        public static bool ForceRemove(string fullPath)
        {

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            else if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                return true;
            }
            else
            {
                SystemIO.STDOUT.PutLine(fullPath + " does not exist!");
                return false;
            }
        }

        public static bool ForceCopy(string sourcePath, string destPath)
        {
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destPath, overwrite: true);
                return true;
            }
            else if (Directory.Exists(sourcePath))
            {
                Entries.CopyDirectory(sourcePath, destPath);
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
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destPath, overwrite: true);
            }
        }

        public static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string dest = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, dest);
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                string dest = Path.Combine(destDir, Path.GetFileName(directory));
                CopyDirectory(directory, dest);
            }
        }
    }
}
