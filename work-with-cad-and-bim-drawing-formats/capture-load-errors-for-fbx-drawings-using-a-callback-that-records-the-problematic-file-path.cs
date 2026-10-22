using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadLoadDiagnostics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Directory containing FBX files
            string inputDirectory = @"C:\CadFiles";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Collect file paths with load errors
            List<string> filesWithErrors = new List<string>();

            // Get all FBX files in the directory
            string[] fbxFiles = Directory.GetFiles(inputDirectory, "*.fbx", SearchOption.AllDirectories);

            foreach (string filePath in fbxFiles)
            {
                // Guard against missing file (should not happen as GetFiles returns existing files)
                if (!File.Exists(filePath))
                {
                    // Create an empty placeholder to satisfy validation rules
                    try
                    {
                        File.WriteAllBytes(filePath, new byte[0]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to create placeholder for missing file '{filePath}': {ex.Message}");
                        continue;
                    }
                }

                // Configure load options to ignore errors during loading
                LoadOptions loadOptions = new LoadOptions
                {
                    IgnoreErrors = true
                };

                try
                {
                    // Attempt to load the CAD drawing
                    using (Image cadImage = Image.Load(filePath, loadOptions))
                    {
                        // No further processing required; just ensure the image is disposed
                    }
                }
                catch (Exception)
                {
                    // An exception indicates a critical load failure – record the file path
                    filesWithErrors.Add(filePath);
                    continue;
                }

                // After loading, inspect the Errors collection for any non‑critical issues
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    filesWithErrors.Add(filePath);
                }
            }

            // Output the results
            Console.WriteLine("CAD files with load errors:");
            foreach (string errorFile in filesWithErrors)
            {
                Console.WriteLine(errorFile);
            }
        }
    }
}