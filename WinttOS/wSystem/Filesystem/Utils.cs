﻿using System.IO;
using WinttOS.Core;

namespace WinttOS.wSystem.Filesystem
{
    public sealed class Utils
    {
        public static string GetParentPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            path = IOMapper.MapFHSToPhysical(path);

            if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                path = path.TrimEnd(Path.DirectorySeparatorChar);
            }

            int lastSeparatorIndex = path.LastIndexOf(Path.DirectorySeparatorChar);
            if (lastSeparatorIndex <= 0)
            {
                return path + "\\";
            }
            if (lastSeparatorIndex == 2 && path[1] == ':')
            {
                return IOMapper.MapPhysicalToFHS(path.Substring(0, lastSeparatorIndex + 1));
            }
            return IOMapper.MapPhysicalToFHS(path.Substring(0, lastSeparatorIndex) + "\\");
        }

        public static string GetFreeSpace()
        {
            var available_space = GlobalData.FileSystem.GetAvailableFreeSpace(GlobalData.CurrentVolume);
            return ConvertSize(available_space);
        }

        public static string GetCapacity()
        {
            var total_size = GlobalData.FileSystem.GetTotalSize(GlobalData.CurrentVolume);
            return ConvertSize(total_size);
        }

        public static string ConvertSize(long bytes)
        {
            string suffix = " Bytes";
            double size = bytes;

            if (size >= 1024 * 1024 * 1024)
            {
                size /= 1024 * 1024 * 1024;
                suffix = "GB";
            }
            else if (size >= 1024 * 1024)
            {
                size /= 1024 * 1024;
                suffix = "MB";
            }
            else if (size >= 1024)
            {
                size /= 1024;
                suffix = "KB";
            }

            return $"{Round(size)}{suffix}";
        }

        private static string Round(double number)
        {
            string numStr = number.ToString();
            int dotIndex = numStr.IndexOf('.');

            if (dotIndex != -1)
            {
                return numStr.Substring(0, dotIndex);
            }
            return numStr;
        }
    }
}
