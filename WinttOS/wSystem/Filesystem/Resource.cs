using System;

namespace WinttOS.wSystem.Filesystem
{
    internal class Resource
    {
        public string Path { get; }
        public Action<byte[]> LoadCallback { get; }
        public string Description { get; }

        public Resource(string path, Action<byte[]> loadCallback, string description)
        {
            Path = path;
            LoadCallback = loadCallback;
            Description = description;
        }
    }
}
