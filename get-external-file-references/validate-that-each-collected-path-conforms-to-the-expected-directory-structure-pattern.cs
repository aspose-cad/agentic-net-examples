using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // Expected pattern: output\<subfolder>\filename.{dwg|dxf|dgn}
        const string pattern = @"^output\\[A-Za-z0-9_]+\\[^\\]+\.((dwg)|(dxf)|(dgn))$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);

        if (args.Length == 0)
        {
            Console.WriteLine("No file paths provided.");
            return;
        }

        foreach (string rawPath in args)
        {
            // Normalize path separators for consistency
            string path = rawPath.Replace('/', '\\');

            // Validate directory structure
            if (!regex.IsMatch(path))
            {
                Console.WriteLine($"Path does not match expected pattern: {rawPath}");
                continue;
            }

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create directory '{directory}': {ex.Message}");
                    continue;
                }
            }

            // Verify file existence; create a placeholder if missing
            if (!File.Exists(path))
            {
                try
                {
                    // Create a minimal placeholder file
                    File.WriteAllText(path, "Placeholder CAD content");
                    Console.WriteLine($"Created placeholder file: {rawPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create placeholder for '{rawPath}': {ex.Message}");
                    continue;
                }
            }

            Console.WriteLine($"Valid path and file ready: {rawPath}");
        }
    }
}