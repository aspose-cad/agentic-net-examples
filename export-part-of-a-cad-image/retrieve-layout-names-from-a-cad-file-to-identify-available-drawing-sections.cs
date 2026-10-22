using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace AsposeCadLayoutNames
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input CAD file path
                string inputPath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD file
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific properties
                    CadImage cadSpecificImage = (CadImage)cadImage;

                    // Retrieve layout names (dictionary keys)
                    string[] layoutNames = cadSpecificImage.Layouts.Keys.ToArray();

                    // Prepare output directory
                    string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                    Directory.CreateDirectory(outputDir);

                    // Write layout names to a text file
                    string outputPath = Path.Combine(outputDir, "layouts.txt");
                    File.WriteAllLines(outputPath, layoutNames);

                    Console.WriteLine($"Layout names have been saved to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}