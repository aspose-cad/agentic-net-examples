using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string sourcePath = "input.dwg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"File not found: {sourcePath}");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(sourcePath))
            {
                // Cast the loaded image to CadImage to access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Iterate through top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Identify DGN underlay entities
                    if (entity is CadDgnUnderlay dgnUnderlay)
                    {
                        // Check the UnderlayPath property
                        string underlayPath = dgnUnderlay.UnderlayPath;
                        if (string.IsNullOrEmpty(underlayPath))
                        {
                            Console.WriteLine("Warning: CadDgnUnderlay entity has an empty UnderlayPath.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}