using System.CommandLine;
using CLIProjectTool.Interfaces;

namespace CLIProjectTool.Commands
{
    public class ListCommand : ICommand
    {
        static readonly string destinationFolderPath = Path.Combine(Environment.CurrentDirectory, "Templates");

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
            try
            {
                string[] filePaths = Directory.GetFiles(destinationFolderPath);
                if (filePaths.Length == 0)
                {
                    Console.WriteLine("The Templates folder contains no templates");
                }
                string?[] files = filePaths.Select(Path.GetFileName).ToArray();

                Console.WriteLine($"\nFiles in Templates: ");
                foreach (string? file in files)
                {
                    if (file == null) continue;
                    if (file == ".gitignore") continue;
                    if (pattern == null || file.Contains(pattern, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Console.WriteLine("-> " + file);
                    }
                }
                
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.Message);
            }
        }
    }
}