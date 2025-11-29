using System.CommandLine;
using System.ComponentModel;
using System.Text.Json;
using CLIProjectTool.Interfaces;
using CLIProjectTool.Models;
using CLIProjectTool.Services;

namespace CLIProjectTool.Commands
{
    public class TemplateCommand : ICommand
    {
        static readonly string destinationFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");

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
            string createDir = Path.Combine(destinationFolderPath, name);
            string metaDataPath = Path.Combine(createDir, "template.json");
            try
            {
                // Create Directory
                if (!Directory.Exists(createDir))
                {
                    Directory.CreateDirectory(createDir);
                    Console.WriteLine($"Created directory: {createDir}");
                }

                // Create .json Meta data 
                if (!File.Exists(metaDataPath))
                {
                    using (File.Create(metaDataPath))
                    {
                    }
                }
                
                TemplateMetadata newTemp = new TemplateMetadata
                {
                    Name = name
                }; 
                string jsonString = JsonSerializer.Serialize(newTemp, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(metaDataPath, jsonString);

                // Copy over single file or folder for template
                if (Directory.Exists(path))
                {
                    string desFolderPath = Path.Combine(createDir, name);
                    TemplateService.CopyFolder(path, desFolderPath, true);
                }
                else if (File.Exists(path))
                {
                    string desFilePath = Path.Combine(createDir, name + Path.GetExtension(path));
                    TemplateService.CopyFile(path, createDir, desFilePath);
                }
            }   
            catch (Exception err)
            {
                Console.Error.WriteLine(err.Message);
            }
            
        }

        private static void HandleRemove(string name)
        {
            // Find matching template file (with any extension)
            string[] matchingFiles;
            try
            {
                if (!Directory.Exists(destinationFolderPath))
                {
                    Console.Error.WriteLine($"Templates folder not found: {destinationFolderPath}");
                    return;
                }

                matchingFiles = Directory.GetFiles(destinationFolderPath, $"{name}.*");
                if (matchingFiles.Length == 0)
                {
                    // Try exact match (no extension)
                    string exactPath = Path.Combine(destinationFolderPath, name);
                    if (File.Exists(exactPath))
                    {
                        matchingFiles = new[] { exactPath };
                    }
                }
            }
            catch (Exception err)
            {
                Console.Error.WriteLine($"Error searching for template: {err.Message}");
                return;
            }

            if (matchingFiles.Length == 0)
            {
                Console.Error.WriteLine($"No template found with name: {name}");
                return;
            }

            foreach (var file in matchingFiles)
            {
                try
                {
                    File.Delete(file);
                    Console.WriteLine($"Removed template: {Path.GetFileName(file)}");
                }
                catch (Exception err)
                {
                    Console.Error.WriteLine($"Failed to remove {Path.GetFileName(file)}: {err.Message}");
                }
            }
        }
    }
}