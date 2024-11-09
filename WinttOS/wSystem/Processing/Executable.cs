using LunarLabs.Parser;
using LunarLabs.Parser.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WinttOS.wSystem.Compression;

namespace WinttOS.wSystem.Processing
{
    public sealed class Executable
    {
        private const string ExpectedSignature = "CEXE";
        private const string ExpectedLibSignature = "LIB_";
        private const int SignatureSize = 4;
        private const int ArchiveSizeLength = 4;

        public string Signature { get; private set; }
        public bool IsLibrary { get; private set; }
        public string Name { get; private set; }
        public List<string> Dependencies { get; private set; }
        public byte[] RawData { get; private set; }
        public int ArchiveSize { get; private set; }
        public Dictionary<string, byte[]> LuaSources { get; set; }
        private byte[] ZipContent { get; set; }

        public Executable(byte[] executableBytes)
        {
            RawData = executableBytes;
            LuaSources = new Dictionary<string, byte[]>();
            Dependencies = new List<string>();
            ParseExecutable(executableBytes);
        }

        private void ParseExecutable(byte[] executableBytes)
        {
            Signature = Encoding.ASCII.GetString(executableBytes, 0, SignatureSize);

            if (Signature != ExpectedSignature && Signature != ExpectedLibSignature)
            {
                throw new InvalidOperationException("This is not a Cosmos executable.");
            }

            IsLibrary = Signature == ExpectedLibSignature;

            ArchiveSize = BitConverter.ToInt32(executableBytes, SignatureSize);
            if (SignatureSize + ArchiveSizeLength + ArchiveSize > executableBytes.Length)
            {
                throw new InvalidOperationException("Cosmos executable corrupted.");
            }

            ZipContent = new byte[ArchiveSize];

            Array.Copy(executableBytes, SignatureSize + ArchiveSizeLength, ZipContent, 0, ArchiveSize);

            ExtractLuaScripts();
        }

        private void ExtractLuaScripts()
        {
            bool mainFound = false;

            using (MemoryStream zipStream = new MemoryStream(ZipContent))
            {
                using (ZipStorer zip = ZipStorer.Open(zipStream, FileAccess.Read))
                {
                    List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();

                    foreach (ZipStorer.ZipFileEntry entry in dir)
                    {
                        using (MemoryStream fileStream = new MemoryStream())
                        {
                            zip.ExtractFile(entry, fileStream);
                            byte[] script = fileStream.ToArray();
                            if (entry.FilenameInZip == "package.json")
                            {
                                string json = Encoding.UTF8.GetString(script);

                                DataNode root = JSONReader.ReadFromString(json);

                                foreach(var node in root["dependencies"])
                                {
                                    if (node == null)
                                        break;
                                    Dependencies.Add(node.Value);
                                }

                                Name = root["name"].Value;
                                continue;
                            }

                            LuaSources.Add(entry.FilenameInZip, script);

                            if (entry.FilenameInZip == "main.lua")
                            {
                                mainFound = true;
                            }
                        }
                    }
                }
            }

            if (!mainFound && !IsLibrary)
            {
                throw new Exception("Could not find 'main.lua' in the executable.");
            }
        }
    }
}
