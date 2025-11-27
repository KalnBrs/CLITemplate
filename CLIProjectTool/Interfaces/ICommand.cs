using System.CommandLine;

namespace CLIProjectTool.Interfaces
{
    /// <summary>
    /// Marker interface for CLI commands.
    /// Each command class should implement a static Build() method returning a Command.
    /// </summary>
    public interface ICommand
    {
        // Each implementing class provides:
        // static Command Build();
    }
}