using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CadLayoutNames
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Determine input file path
                string inputPath = args.Length > 0 ? args[0] : "sample.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Error: Input file \"{inputPath}\" does not exist.");
                    return;
                }

                // Prepare output directory
                string outputDirectory = Path.Combine(Path.GetDirectoryName(inputPath) ?? string.Empty, "output");
                Directory.CreateDirectory(outputDirectory);
                string outputPath = Path.Combine(outputDirectory, "layout_names.txt");

                // Load CAD file
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Collect layout names
                    List<string> layoutNames = new List<string>();
                    foreach (var layout in cadImage.Layouts.Values)
                    {
                        // Attempt to get the Name property; fallback to empty string if not present
                        var nameProp = layout.GetType().GetProperty("Name");
                        string name = nameProp?.GetValue(layout)?.ToString() ?? string.Empty;
                        layoutNames.Add(name);
                    }

                    // Write layout names to file
                    File.WriteAllLines(outputPath, layoutNames);
                    Console.WriteLine($"Layout names have been written to \"{outputPath}\".");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}