using System.CommandLine;
using CLIProjectTool.Interfaces;
using System.Diagnostics;
using System.Reflection;

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
            // Prefer informational version (NuGet/package version), fall back to file/assembly versions
            var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            var infoVersion = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            string? fileVersion = null;

            try
            {
                // FileVersionInfo may work even when assembly version isn't set
                fileVersion = FileVersionInfo.GetVersionInfo(asm.Location).ProductVersion;
            }
            catch
            {
                // ignore — asm.Location can be empty in some publish modes
            }

            var assemblyVersion = asm.GetName().Version?.ToString();
            var version = infoVersion ?? fileVersion ?? assemblyVersion ?? "unknown";

            Console.WriteLine($"Version {version}");
        }
    }
}