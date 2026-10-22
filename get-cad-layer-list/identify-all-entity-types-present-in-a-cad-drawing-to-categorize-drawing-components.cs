using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityInspector
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder input CAD file path
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found - {inputPath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Use a HashSet to collect distinct entity type names
                    HashSet<string> entityTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    // Iterate over all entities in the drawing
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Get the simple name of the entity's type (e.g., Line, Circle, Polyline)
                        string typeName = entity.GetType().Name;
                        entityTypes.Add(typeName);
                    }

                    // Output the identified entity types
                    Console.WriteLine("Entity types present in the drawing:");
                    foreach (string typeName in entityTypes)
                    {
                        Console.WriteLine($"- {typeName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}