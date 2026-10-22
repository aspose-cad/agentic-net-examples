using System;
using System.IO;
using System.Diagnostics;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder paths – replace with actual directories.
            string inputDirectory = "input_stl";
            string outputDirectory = "output_png";

            // Verify input directory exists.
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists.
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Get all STL files in the input directory.
            string[] stlFiles = Directory.GetFiles(inputDirectory, "*.stl", SearchOption.TopDirectoryOnly);
            if (stlFiles.Length == 0)
            {
                Console.Error.WriteLine($"No STL files found in: {inputDirectory}");
                return;
            }

            Process currentProcess = Process.GetCurrentProcess();

            foreach (string stlPath in stlFiles)
            {
                // Measure memory before loading.
                currentProcess.Refresh();
                long memoryBefore = currentProcess.PrivateMemorySize64;

                // Load the STL file.
                using (Image cadImage = Image.Load(stlPath))
                {
                    // Prepare PNG save options.
                    PngOptions pngOptions = new PngOptions();

                    // Determine output PNG path.
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(stlPath);
                    string pngPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".png");

                    // Save as PNG.
                    cadImage.Save(pngPath, pngOptions);
                }

                // Force garbage collection to release resources.
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Measure memory after conversion.
                currentProcess.Refresh();
                long memoryAfter = currentProcess.PrivateMemorySize64;

                long memoryDiff = memoryAfter - memoryBefore;
                Console.WriteLine($"Converted '{Path.GetFileName(stlPath)}' to PNG. " +
                                  $"Memory before: {memoryBefore / (1024 * 1024)} MB, " +
                                  $"after: {memoryAfter / (1024 * 1024)} MB, " +
                                  $"diff: {memoryDiff / (1024 * 1024)} MB.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}