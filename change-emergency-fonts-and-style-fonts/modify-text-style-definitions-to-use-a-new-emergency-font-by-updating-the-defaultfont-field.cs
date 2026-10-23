using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;

namespace CadExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                string inputPath = "input.dwg";
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                string outputDir = "output";
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "modified.dwg");

                using (Image image = Image.Load(inputPath))
                {
                    if (image is CadImage cadImage)
                    {
                        // Update the default font to the new emergency font
                        string newEmergencyFont = "Arial Unicode MS";
                        cadImage.DefaultFont = newEmergencyFont;

                        // Save the modified CAD file
                        cadImage.Save(outputPath);
                        Console.WriteLine($"Modified CAD file saved to: {outputPath}");
                    }
                    else
                    {
                        Console.Error.WriteLine("Loaded file is not a CAD image.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}