using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityCountVerification
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD drawing file (DWG/DXF)
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to CadImage to access CAD‑specific members
                    CadImage cadImage = (CadImage)image;

                    // Counter for processed entities
                    int processedEntityCount = 0;

                    // Iterate over top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Example processing: output entity type name
                        Console.WriteLine($"Entity Type: {entity.GetType().Name}");
                        processedEntityCount++;
                    }

                    // Verify that the number of processed entities matches the drawing's entity count
                    int totalEntityCount = cadImage.Entities.Count();
                    bool countsMatch = processedEntityCount == totalEntityCount;

                    Console.WriteLine($"Processed Entities: {processedEntityCount}");
                    Console.WriteLine($"Total Entities in Drawing: {totalEntityCount}");
                    Console.WriteLine($"Counts match: {countsMatch}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}