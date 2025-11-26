using System.CommandLine;
using System.Reflection.Metadata.Ecma335;
using CLIProjectTool.Interfaces;

namespace CLIProjectTool.Commands
{
    public class TemplateCommand : ICommand
    {
        static readonly string destinationFolderPath = Path.Combine(Environment.CurrentDirectory, "Templates");

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
            string desFilePath = Path.Combine(destinationFolderPath, name + Path.GetExtension(path));
            Console.WriteLine(destinationFolderPath);
            Console.WriteLine($"This adds a tempate with this name {desFilePath}, and this path {path}");

            try 
            {
                if (!Directory.Exists(destinationFolderPath))
                {
                    Directory.CreateDirectory(destinationFolderPath);
                    Console.WriteLine($"Created directory: {destinationFolderPath}");
                }

                if (!File.Exists(desFilePath))
                {
                    using (File.Create(desFilePath))
                    {
                    }
                }

                File.Copy(path, desFilePath, true);
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.Message);
            }
        }

        private static void HandleRemove(string name)
        {
            Console.WriteLine($"This removes a tempate with this name {name}");
        }
    }
}