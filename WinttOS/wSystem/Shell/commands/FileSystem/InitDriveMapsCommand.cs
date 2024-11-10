using WinttOS.wSystem.Filesystem;

namespace WinttOS.wSystem.Shell.commands.FileSystem
{
    internal class InitDriveMapsCommand : Command
    {
        public InitDriveMapsCommand(string[] commandValues) : base(commandValues)
        {
            Description = "Initialiazes drive mapping configuration";
        }

        public override ReturnInfo Execute()
        {
            IOMapper.SaveMapping();
            return new(this, ReturnCode.OK);
        }
    }
}
