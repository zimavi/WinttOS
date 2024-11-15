using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Shell.Programs;

namespace WinttOS.wSystem.Shell.commands.FileSystem
{
    public class MiniDiffCommand : Command
    {
        public MiniDiffCommand() : base(new string[] { "mini-diff", "mdiff"})
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "make")
            {
                if (arguments.Count < 4)     // make file file file
                {
                    return new(this, ReturnCode.ERROR, "Use: mdiff make [file_orig] [file_new] [diff_file]");
                }

                if (!arguments[1].StartsWith('/'))
                {
                    arguments[1] = GlobalData.CurrentDirectory + arguments[1];
                }
                if (!File.Exists(IOMapper.MapFHSToPhysical(arguments[1])))
                {
                    return new(this, ReturnCode.ERROR, "File does not exists: " + arguments[1]);
                }

                if (!arguments[2].StartsWith('/'))
                {
                    arguments[2] = GlobalData.CurrentDirectory + arguments[2];
                }
                if (!File.Exists(IOMapper.MapFHSToPhysical(arguments[2])))
                {
                    return new(this, ReturnCode.ERROR, "File does not exists: " + arguments[2]);
                }

                string[] file1 = File.ReadAllLines(IOMapper.MapFHSToPhysical(arguments[1]));
                string[] file2 = File.ReadAllLines(IOMapper.MapFHSToPhysical(arguments[2]));

                List<string> diff = MiniDiff.GetDifferences(file1, file2, arguments[1], arguments[2]);

                if (!arguments[3].StartsWith('/'))
                {
                    arguments[3] = GlobalData.CurrentDirectory + arguments[3];
                }
                File.WriteAllLines(IOMapper.MapFHSToPhysical(arguments[3]), diff.ToArray());
                SystemIO.STDOUT.PutLine("Diff file created.");
            }
            else if (arguments[0] == "patch")
            {
                if (arguments.Count < 3)     // patch file file
                {
                    return new(this, ReturnCode.ERROR, "Use: mdiff patch [file_to_patch] [diff_file]");
                }

                if (!arguments[1].StartsWith('/'))
                {
                    arguments[1] = GlobalData.CurrentDirectory + arguments[1];
                }
                if (!File.Exists(IOMapper.MapFHSToPhysical(arguments[1])))
                {
                    return new(this, ReturnCode.ERROR, "File does not exists: " + arguments[1]);
                }

                if (!arguments[2].StartsWith('/'))
                {
                    arguments[2] = GlobalData.CurrentDirectory + arguments[2];
                }
                if (!File.Exists(IOMapper.MapFHSToPhysical(arguments[2])))
                {
                    return new(this, ReturnCode.ERROR, "File does not exists: " + arguments[2]);
                }

                string patchData = File.ReadAllText(IOMapper.MapFHSToPhysical(arguments[2]));
                string data = File.ReadAllText(IOMapper.MapFHSToPhysical(arguments[1]));

                List<string> patch = MiniDiff.ApplyPatch(patchData, data);

                File.WriteAllLines(IOMapper.MapFHSToPhysical(arguments[1]), patch.ToArray());
                SystemIO.STDOUT.PutLine("Patch applied.");
            }
            return new(this, ReturnCode.OK);
        }
    }
}
