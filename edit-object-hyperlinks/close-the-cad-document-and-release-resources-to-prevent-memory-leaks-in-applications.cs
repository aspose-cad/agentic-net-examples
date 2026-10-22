using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
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
        // Load the CAD document inside a using block to ensure it is disposed properly
                using (Aspose.CAD.Image cadImage = Aspose.CAD.Image.Load(inputPath))
                {
                    // Example operation: save the CAD drawing to PNG format
                    string outputPath = "output.png";
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(outputPath, pngOptions);
                } // cadImage is automatically closed and resources are released here
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}