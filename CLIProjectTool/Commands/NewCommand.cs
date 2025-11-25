using System.CommandLine;
using CLIProjectTool.Interfaces;

namespace CLIProjectTool.Commands
{
    public class NewCommand : ICommand
    {
        public static Command Build()
        {
            var templateArgument = new Argument<string>("templateName", "the template the user wants to use");
            var projectNameArgument = new Argument<string>("projectName", "The name of the project");

            var newCommand = new Command("new", "Use a template to create a new project")
            {
            templateArgument,
            projectNameArgument
            };

            newCommand.SetHandler(handleNewCommand, templateArgument, projectNameArgument);

            return newCommand;
        }

        private static void handleNewCommand(string template, string projectName)
        {
            Console.WriteLine($"\nCreated new project with this template: {template}, and this project name: {projectName}");
        }
    }
}