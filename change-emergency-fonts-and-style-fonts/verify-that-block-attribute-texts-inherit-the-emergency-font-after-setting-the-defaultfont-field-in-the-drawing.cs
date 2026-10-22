using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Set the emergency default font for the drawing
                cadImage.DefaultFont = "Arial";

                // Iterate through all block entities in the drawing
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Iterate through entities contained in the current block
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        // Output block name and entity type to verify font inheritance
                        Console.WriteLine($"Block: {block.Name}, Entity Type: {entity.GetType().Name}");
                    }
                }

                // Save the modified drawing while preserving editable objects
                string outputPath = "output.dxf";
                DxfOptions saveOptions = new DxfOptions { OutputMode = CadOutputMode.Convert };
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}