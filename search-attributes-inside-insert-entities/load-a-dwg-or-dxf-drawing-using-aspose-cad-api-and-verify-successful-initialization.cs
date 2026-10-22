using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Path to the CAD file (DWG or DXF). Replace with a valid path or use a placeholder.
        string filePath = "sample.dwg";

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(filePath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Verify successful initialization by checking the number of top-level entities
                int entityCount = cadImage.Entities.Count();
                Console.WriteLine($"CAD drawing loaded successfully. Entity count: {entityCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}