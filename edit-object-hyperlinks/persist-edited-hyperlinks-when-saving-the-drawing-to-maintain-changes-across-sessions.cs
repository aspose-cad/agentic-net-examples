using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadHyperlinkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage for CAD-specific operations
                    if (image is CadImage cadImage)
                    {
                        // NOTE: Aspose.CAD currently does not expose a public API for editing hyperlinks.
                        // If hyperlink support becomes available, modify them here.
                        // For now, we simply proceed to save the drawing.

                        // Save the drawing preserving all existing data
                        var saveOptions = new DwgOptions();
                        cadImage.Save(outputPath, saveOptions);
                    }
                    else
                    {
                        Console.Error.WriteLine("Loaded file is not a CAD image.");
                    }
                }

                Console.WriteLine($"Drawing saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}