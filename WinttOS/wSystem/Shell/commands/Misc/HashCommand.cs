using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using WinttOS.Core;
using WinttOS.Core.Utils.Cryptography;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.Misc
{
    public class HashCommand : Command
    {
        public HashCommand(string[] commandValues) : base(commandValues)
        {
            CommandManual = new()
            {
                "HASH - Compute hash values for text or files",
                "",
                "SYNOPSIS",
                "    hash [--sha256] [--md5] [--text | -t] [--file | -f] [input ...]",
                "",
                "DESCRIPTION",
                "    The hash command computes hash values for either text strings or files, using either the SHA-256 or MD5 algorithm.",
                "",
                "OPTIONS",
                "    --text, -t",
                "        Specifies that the input to be hashed will be computed from text strings following this flag.",
                "",
                "    --file, -f",
                "        Specifies that the input to be hashed will be computed from a file contents of path following this flag.",
                "",
                "    --sha256",
                "        Specifies that the SHA-256 algorithm should be used to compute the hash value.",
                "",
                "    --md5",
                "        Specifies that the MD5 algorithm should be used to compute the hash value.",
                "",
                "    input",
                "        For the --text option, input is expected to be provided directly after the flags and is treated as the text string to be hashed.",
                "        For the --file option, input is expected to be the path to the file to be hashed.",
                "",
                "EXAMPLES",
                "    To compute the SHA-256 hash of a text string:",
                "        $ hash --sha256 --text \"hello world\"",
                "",
                "    To compute the MD5 checksum of a file:",
                "        $ hash --md5 --file /path/to/file.txt",
                "",
                "AUTHOR",
                "    Written by zimavi"
            };
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("hash [--sha256] [--md5] [--text | -t] [--file | -f] [input ...]");
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments.Count < 3)
                return new ReturnInfo(this, ReturnCode.ERROR_ARG);

            bool isSha;
            bool isFile;

            switch (arguments[0])
            {
                case "--sha256":
                    isSha = true; 
                    break;
                case "--md5":
                    isSha = false;
                    break;
                default:
                    return new(this, ReturnCode.ERROR_ARG);
            }

            switch(arguments[1])
            {
                case "--text":
                case "-t":
                    isFile = false;
                    break;
                case "--file":
                case "-f":
                    isFile = true;
                    break;
                default:
                    return new(this, ReturnCode.ERROR_ARG);
            }

            List<string> data;
            string hash;

            if (isSha)
            {
                if (isFile)
                {
                    if (!arguments[2].StartsWith('/'))
                        arguments[2] = GlobalData.CurrentDirectory + arguments[2];
                    if (!File.Exists(IOMapper.MapFHSToPhysical(arguments[2])))
                    {
                        return new(this, ReturnCode.ERROR, "Cannot find file.");
                    }

                    string contents = File.ReadAllText(IOMapper.MapFHSToPhysical(arguments[2]));

                    string checksum = Sha256.hash(contents);

                    SystemIO.STDOUT.PutLine("File checksum:\n" + checksum);

                    return new(this, ReturnCode.OK);
                }

                data = arguments.SubList(3, arguments.Count - 3);
                hash = Sha256.hash(string.Join(' ', data.ToArray()));

                SystemIO.STDOUT.PutLine("Hash:\n" + hash);
                return new(this, ReturnCode.OK);
            }
            if (isFile)
            {
                if (!arguments[2].StartsWith('/'))
                    arguments[2] = GlobalData.CurrentDirectory + arguments[2];
                if (!File.Exists(IOMapper.MapFHSToPhysical(arguments[2])))
                {
                    return new(this, ReturnCode.ERROR, "Cannot find file.");
                }

                string contents = File.ReadAllText(IOMapper.MapFHSToPhysical(arguments[2]));

                string checksum = MD5.hash(contents);

                SystemIO.STDOUT.PutLine("File checksum:\n" + checksum);

                return new(this, ReturnCode.OK);
            }

            data = arguments.SubList(3, arguments.Count - 3);
            hash = MD5.hash(string.Join(' ', data.ToArray()));

            SystemIO.STDOUT.PutLine("Hash:\n" + hash);
            return new(this, ReturnCode.OK);
        }
    }
}
