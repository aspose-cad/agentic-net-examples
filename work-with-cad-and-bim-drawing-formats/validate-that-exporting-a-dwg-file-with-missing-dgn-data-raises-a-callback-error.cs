using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportValidation
{
    class Program
    {
        static void Main()
        {
            try
            {
                string inputPath = "sample.dwg";
                string outputPath = "output.dgn";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the DWG file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members if needed
                    if (image is not CadImage cadImage)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Attempt to save as DGN; expect an error due to missing DGN data
                    try
                    {
                        // Using default save options; Aspose.CAD will throw if data is insufficient
                        cadImage.Save(outputPath);
                        Console.WriteLine("Export completed without errors (unexpected).");
                    }
                    catch (Exception exportEx)
                    {
                        // Expected error handling
                        Console.WriteLine($"Export error captured: {exportEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}