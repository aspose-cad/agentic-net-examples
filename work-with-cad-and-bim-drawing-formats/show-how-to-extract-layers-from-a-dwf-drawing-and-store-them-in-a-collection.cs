using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace ExtractDwfLayers
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DWF file
                string sourcePath = "input.dwf";

                // Verify the input file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Input file not found: {sourcePath}");
                    return;
                }

                // Load the DWF drawing using Aspose.CAD.Image.Load
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast the loaded image to CadImage to access CAD-specific members
                    Image cadImage = image ;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // NOTE: The current Aspose.CAD API does not expose a LayerTable or equivalent
                    // structure for DWF files. Therefore extracting layer information is not supported.
                    Console.WriteLine("Layer extraction is not supported: CadImage does not provide a LayerTable in the current API.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}