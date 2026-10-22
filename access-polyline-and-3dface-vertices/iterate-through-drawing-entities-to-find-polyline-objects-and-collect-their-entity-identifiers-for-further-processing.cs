using System;
using System.Collections.Generic;
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

            // List to store identifiers of polyline entities
            List<string> polylineIds = new List<string>();

            // Load the CAD drawing
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("The loaded file is not a supported CAD format.");
                    return;
                }

                // Iterate through all top‑level entities in the drawing
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Check if the entity is a lightweight polyline
                    if (entity is CadLwPolyline)
                    {
                        // Collect the entity identifier (Id)
                        polylineIds.Add(entity.Id);
                    }
                }
            }

            // Output the collected identifiers
            Console.WriteLine($"Found {polylineIds.Count} polyline entities.");
            foreach (string id in polylineIds)
            {
                Console.WriteLine($"Polyline Id: {id}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}