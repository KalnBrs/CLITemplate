using System.CommandLine;
using System.Reflection.Metadata.Ecma335;
using CLIProjectTool.Interfaces;

namespace CLIProjectTool.Commands
{
    public class TemplateCommand : ICommand
    {
        public static Command Build()
        {
            var templateCommand = new Command("templates", "using the templates");
            
            var name = new Argument<string>("name", "name of the new template");
            var path = new Argument<string>("path", "the path to the file to copy");
            var addCommand = new Command("add", "add a template to the templates")
            {
                name,
                path
            };

            

            var removeCommand = new Command("remove", "remove a template")
            {
                name
            };
            
            addCommand.SetHandler(HandleAdd, name, path);
            removeCommand.SetHandler(HandleRemove, name);
            templateCommand.Add(addCommand);
            templateCommand.Add(removeCommand);

            return templateCommand;
        }

        private static void HandleAdd(string name, string path)
        {
            Console.WriteLine($"This adds a tempate with this name {name}, and this path {path}");
        }

        private static void HandleRemove(string name)
        {
            Console.WriteLine($"This removes a tempate with this name {name}");
        }
    }
}