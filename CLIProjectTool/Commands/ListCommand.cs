using System.CommandLine;
using CLIProjectTool.Interfaces;

namespace CLIProjectTool.Commands
{
    public class ListCommand : ICommand
    {
        public static Command Build()
        {
            var patternOption = new Option<string?>(
                name: "--pattern",
                description: "A pattern to filter the templates/scripts list"
            );

            var listCommand = new Command("list", "Lists all the templates/scripts")
            {
                patternOption
            };

            listCommand.SetHandler(HandleListCommand, patternOption);
            return listCommand;
        }

        private static void HandleListCommand(string? pattern)
        {
            Console.WriteLine($"\nHello, listing all the scripts/templates (filter: '{pattern ?? "none"}')");
        }
    }
}