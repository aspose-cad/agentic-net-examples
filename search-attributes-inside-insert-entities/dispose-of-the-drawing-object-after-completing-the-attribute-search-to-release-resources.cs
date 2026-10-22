using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadBlockAttributeSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD drawing file (DWG/DXF)
                string inputPath = "sample.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the drawing and ensure it is disposed after use
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Iterate through all block definitions in the drawing
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block Name: {block.Name}");

                        // Iterate through entities contained in the current block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            // The SDK does not expose a dedicated CadAttribute class.
                            // Attribute entities can be identified by their type name.
                            if (entity.TypeName.ToString().Equals("ATTRIB", StringComparison.OrdinalIgnoreCase))
                            {
                                // Specific attribute properties such as Tag or TextString are not available
                                // via the current Aspose.CAD API. We can only indicate that an attribute
                                // entity was found.
                                Console.WriteLine("  Attribute entity found (details unavailable in SDK).");
                            }
                        }
                    }
                }

                // At this point the CadImage (and underlying Image) has been disposed,
                // releasing all resources associated with the drawing.
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}