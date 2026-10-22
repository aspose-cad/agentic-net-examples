using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to the base drawing, the drawing to insert, and the output composite drawing
            string baseDrawingPath = "base.dwg";
            string insertDrawingPath = "insert.dxf";
            string outputDrawingPath = "composite.dwg";

            // Verify that input files exist
            if (!File.Exists(baseDrawingPath))
            {
                Console.Error.WriteLine($"Base drawing not found: {baseDrawingPath}");
                return;
            }

            if (!File.Exists(insertDrawingPath))
            {
                Console.Error.WriteLine($"Insert drawing not found: {insertDrawingPath}");
                return;
            }

            // Load both CAD files
            using (Image baseImage = Image.Load(baseDrawingPath))
            using (Image insertImage = Image.Load(insertDrawingPath))
            {
                // Cast loaded images to CadImage to access CAD object model
                CadImage baseCadImage = baseImage as CadImage;
                CadImage insertCadImage = insertImage as CadImage;

                if (baseCadImage == null || insertCadImage == null)
                {
                    Console.Error.WriteLine("One or both files could not be loaded as CAD images.");
                    return;
                }

                // Insert entities from the second drawing into the first drawing
                var combinedEntities = baseCadImage.Entities.Concat(insertCadImage.Entities).ToArray();
                baseCadImage.Entities = combinedEntities;

                // Save the composite drawing using DWG format options
                var dwgSaveOptions = new DwgOptions();
                baseCadImage.Save(outputDrawingPath, dwgSaveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}