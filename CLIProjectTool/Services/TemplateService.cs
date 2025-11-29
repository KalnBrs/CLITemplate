namespace CLIProjectTool.Services
{
    public class TemplateService
    {
        public static void CopyFolder(string sourceDir, string destDir, bool copySubDirs)
        {
            // directories to ignore (case-insensitive)
            var ignoreDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "node_modules",
                "__pycache__",
                ".git",
                "bin",
                "obj"
            };

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDir);
            }

            // If the source directory itself should be ignored, skip it.
            if (ignoreDirs.Contains(dir.Name))
            {
                Console.WriteLine($"Skipping ignored directory: {dir.FullName}");
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, true); // The 'true' parameter allows overwriting existing files
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    // Skip ignored subdirectories
                    if (ignoreDirs.Contains(subdir.Name))
                    {
                    Console.WriteLine($"Skipping ignored subdirectory: {subdir.FullName}");
                    continue;
                    }

                    string tempPath = Path.Combine(destDir, subdir.Name);
                    CopyFolder(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        public static void CopyFile(string sourceDir, string destDir, string destFile)
        {
            try 
            {
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                    Console.WriteLine($"Created directory: {destDir}");
                }

                if (!File.Exists(destFile))
                {
                    using (File.Create(destFile))
                    {
                    }
                }

                File.Copy(sourceDir, destFile, true);
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.Message);
            }
        }
    }
}