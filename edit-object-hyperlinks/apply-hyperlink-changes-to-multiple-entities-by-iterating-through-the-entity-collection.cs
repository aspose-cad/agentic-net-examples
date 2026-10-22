using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System.IO;

namespace CadHyperlinkUpdater
{
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
        // Path for the modified CAD file
                    string outputPath = "output.dwg";

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD object model
                        CadImage cadImage = (CadImage)image;

                        // Iterate through all top‑level entities in the drawing
                        foreach (CadEntityBase entity in cadImage.Entities)
                        {
                            // Apply a hyperlink to each entity
                            // (Replace with your desired URL or logic)
                            entity.Hyperlink = "https://example.com";
                        }

                        // Save the modified drawing
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
}