using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Paths to input and output CAD files
                string inputPath = "input.dxf";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.dxf";

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Iterate through all entities in the drawing
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Identify attribute definition entities
                        if (entity.TypeName == CadEntityTypeName.ATTDEF)
                        {
                            Console.WriteLine("Found an attribute definition (ATTDEF) entity.");
                        }
                    }

                    // Save the drawing (optional, using default DXF options)
                    DxfOptions saveOptions = new DxfOptions();
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