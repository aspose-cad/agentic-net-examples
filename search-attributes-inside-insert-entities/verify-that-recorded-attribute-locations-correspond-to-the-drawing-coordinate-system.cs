using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadAttributeVerification
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD drawing file (DWG/DXF)
                string inputPath = "drawing.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate through top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // The Aspose.CAD SDK does not expose a dedicated CadAttribute class
                        // or a direct Position property for attribute entities.
                        // We can identify possible attribute entities by their type name.
                        string typeName = entity.TypeName.ToString();

                        if (typeName.IndexOf("Attribute", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // Since the SDK lacks a specific attribute class, we cannot retrieve
                            // the exact insertion point (X, Y, Z) of the attribute.
                            // The following message indicates this limitation.
                            Console.WriteLine($"Found an attribute-like entity (type: {typeName}), but the SDK does not provide direct access to its position.");
                        }
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