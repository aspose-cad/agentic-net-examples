using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Path to the source CAD file
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path to the output CAD file
                string outputPath = "output.dwg";

                // Load the CAD document
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific properties
                    CadImage cadImage = (CadImage)image;

                    // Change the emergency font for all entities
                    cadImage.DefaultFont = "Arial";

                    // Save the modified CAD document preserving editable objects
                    DwgOptions saveOptions = new DwgOptions();
                    cadImage.Save(outputPath);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}