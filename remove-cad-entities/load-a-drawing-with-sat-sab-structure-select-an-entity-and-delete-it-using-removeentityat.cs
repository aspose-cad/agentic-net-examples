using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityDeletion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source SAT/SAB file
                string inputPath = "input.sat";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Path for the resulting file after deletion
                string outputPath = "output.sat";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the loaded image to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Ensure there is at least one entity to delete
                    var entityList = cadImage.Entities.ToList();
                    if (entityList.Count > 0)
                    {
                        // Delete the first entity
                        entityList.RemoveAt(0);

                        // Replace the Entities collection with the modified list
                        cadImage.Entities = entityList.ToArray();
                    }

                    // Save the modified drawing back to SAT format
                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}