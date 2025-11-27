using System.CommandLine;
using CLIProjectTool.Interfaces;

namespace CLIProjectTool.Commands
{
    public class NewCommand : ICommand
    {
        static readonly string destinationFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");

        public static Command Build()
        {
            var templateArgument = new Argument<string>("templateName", "the template the user wants to use");
            var projectNameArgument = new Argument<string>("projectName", "The name of the project");

            var newCommand = new Command("new", "Use a template to create a new project")
            {
            templateArgument,
            projectNameArgument
            };

            newCommand.SetHandler(HandleNewCommand, templateArgument, projectNameArgument);

            return newCommand;
        }

        private static void HandleNewCommand(string template, string projectName)
        {
            if (string.IsNullOrWhiteSpace(template) || string.IsNullOrWhiteSpace(projectName))
            {
                Console.Error.WriteLine("Template and project name must be provided.");
                return;
            }

            if (projectName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                Console.Error.WriteLine("Project name contains invalid characters.");
                return;
            }

            if (!Directory.Exists(destinationFolderPath))
            {
                Console.Error.WriteLine($"Templates folder not found: {destinationFolderPath}");
                return;
            }

            string templatePath = Path.Combine(destinationFolderPath, template);
            if (!File.Exists(templatePath))
            {
                Console.Error.WriteLine("Did not provide a valid template");
                return;
            }

            string createDir = Path.Combine(Environment.CurrentDirectory, projectName);
            string destFilePath = Path.Combine(createDir, template);

            try
            {
                DirectoryInfo di = Directory.CreateDirectory(createDir);
                Console.WriteLine($"Created (or already existed): {di.FullName}");

                File.Copy(templatePath, destFilePath, overwrite: true);
                Console.WriteLine($"\nCreated new project with template: {template}, project name: {projectName}");
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.Message);
            }
        }
    }
}