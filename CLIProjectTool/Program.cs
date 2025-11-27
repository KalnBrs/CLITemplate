using System.CommandLine;
using CLIProjectTool.Commands;

var rootCommand = new RootCommand("Project Template Builder")
{
  // Set root
  ListCommand.Build(),
  NewCommand.Build(),
  TemplateCommand.Build(),
  VersionCommand.Build()
};

await rootCommand.InvokeAsync(args);