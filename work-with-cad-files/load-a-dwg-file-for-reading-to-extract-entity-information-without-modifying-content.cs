using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the DWG file to be read
                string sourcePath = "sample.dwg"; // TODO: replace with actual file path

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"File not found: {sourcePath}");
                    return;
                }

                // Load the DWG file as a CadImage for read‑only operations
                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // Iterate through all entities in the drawing
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Output basic information about each entity
                        string colorInfo = entity.ColorValue.HasValue
                            ? $"TrueColor: {entity.ColorValue.Value}"
                            : $"IndexedColorId: {entity.ColorId}";
                        Console.WriteLine($"Entity Type: {entity.TypeName}, {colorInfo}");
                    }
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