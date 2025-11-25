using System.CommandLine;
using System.ComponentModel.Design;
using CLIProjectTool.Commands;

var rootCommand = new RootCommand("Project Template Builder")
{
  // Set root
  ListCommand.Build(),
  NewCommand.Build(),
  TemplateCommand.Build()
}; 

await rootCommand.InvokeAsync(args);

