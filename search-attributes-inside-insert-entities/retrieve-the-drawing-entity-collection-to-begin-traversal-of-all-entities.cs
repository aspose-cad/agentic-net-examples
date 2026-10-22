using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityTraversal
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD drawing file
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the loaded image to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Retrieve the collection of top-level drawing entities
                    var entities = cadImage.Entities; // IEnumerable<CadEntityBase>

                    // Begin traversal of all entities
                    foreach (CadEntityBase entity in entities)
                    {
                        // Example: output entity type and layer name
                        Console.WriteLine($"Entity Type: {entity.GetType().Name}, Layer: {entity.LayerName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}