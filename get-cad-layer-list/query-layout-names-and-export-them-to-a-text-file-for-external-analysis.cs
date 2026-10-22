using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.IO;

namespace AsposeCadLayoutExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder input CAD file path
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Prepare output directory and file
                string outputDirectory = "output";
                Directory.CreateDirectory(outputDirectory);
                string outputPath = Path.Combine(outputDirectory, "layouts.txt");

                // Load CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Collect layout names
                    List<string> layoutNames = new List<string>();
                    foreach (var layout in cadImage.Layouts.Values)
                    {
                        layoutNames.Add(layout.LayoutName);
                    }

                    // Write layout names to text file
                    File.WriteAllLines(outputPath, layoutNames);
                }

                Console.WriteLine($"Layout names exported to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}