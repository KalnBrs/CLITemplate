using System.CommandLine;

namespace CLIProjectTool.Interfaces
{
    public interface ICommand
    {
        static Command Build()
        {
            return new Command("Default", "This is no command");
        }
    }
}