using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CAD file to be loaded
            string sourceFile = "sample.dwg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(sourceFile))
            {
                Console.Error.WriteLine($"File not found: {sourceFile}");
                return;
            }

            // Load the CAD drawing into memory using Aspose.CAD
            using (Image cadImage = Image.Load(sourceFile))
            {
                // Cast to CadImage for CAD‑specific operations
                if (cadImage is CadImage cadSpecificImage)
                {
                    // Example processing: output the count of top‑level entities
                    int entityCount = cadSpecificImage.Entities.Count();
                    Console.WriteLine($"Top‑level entity count: {entityCount}");
                }
                else
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}