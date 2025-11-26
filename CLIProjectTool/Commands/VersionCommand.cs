using System.CommandLine;
using CLIProjectTool.Interfaces;

namespace CLIProjectTool.Commands
{
    public class VersionCommand : ICommand
    {
        public static Command Build()
        {
            var versionCommand = new Command("version", "Get the version of the CLI");
            versionCommand.SetHandler(HandleVersionCheck);

            return versionCommand;
        }

        private static void HandleVersionCheck()
        {
            Console.WriteLine("Version 0.0 -- Pre Release");
        }
    }
}