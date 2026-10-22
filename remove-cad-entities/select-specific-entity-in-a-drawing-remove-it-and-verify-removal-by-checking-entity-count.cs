using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
                string inputPath = "sample.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file '{inputPath}' does not exist.");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Get the collection of top‑level entities
                    IEnumerable<CadEntityBase> entities = cadImage.Entities;
                    int initialCount = entities.Count();

                    // Find the first line entity to remove
                    CadLine lineToRemove = entities.OfType<CadLine>().FirstOrDefault();

                    if (lineToRemove != null)
                    {
                        // Build a new array without the selected line
                        CadEntityBase[] updatedEntities = entities
                            .Where(e => !ReferenceEquals(e, lineToRemove))
                            .ToArray();

                        // Replace the entities collection with the updated one
                        cadImage.Entities = updatedEntities;
                    }

                    int finalCount = cadImage.Entities.Count();

                    Console.WriteLine($"Initial entity count: {initialCount}");
                    Console.WriteLine($"Final entity count: {finalCount}");

                    // Save the modified drawing
                    string outputPath = "sample_modified.dwg";
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