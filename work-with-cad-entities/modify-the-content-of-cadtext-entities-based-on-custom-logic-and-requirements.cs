using Aspose.CAD;
using System;
using System.Linq;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using System.IO;

namespace CadTextModifier
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Load the CAD file (DXF, DWG, etc.)
                    string inputPath = "input.dxf";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        CadImage cadImage = (CadImage)Image.Load(inputPath);

                    // Process all entities and modify CadText objects
                    CadEntityBase[] updatedEntities = cadImage.Entities
                        .Select(entity =>
                        {
                            // Identify CadText entities
                            if (entity is CadText originalText)
                            {
                                // Create a new CadText with modified content
                                CadText modifiedText = new CadText
                                {
                                    // Example custom logic: prepend "Modified: " to the original text
                                    DefaultValue = "Modified: " + originalText.DefaultValue,
                                    FirstAlignment = originalText.FirstAlignment,
                                    TextHeight = originalText.TextHeight,
                                    TextRotation = originalText.TextRotation,
                                    // Preserve the original SoftOwner (layout's BlockTableRecordHandle)
                                    SoftOwner = originalText.SoftOwner
                                };
                                return (CadEntityBase)modifiedText;
                            }

                            // Return non‑text entities unchanged
                            return entity;
                        })
                        .ToArray();

                    // Replace the image's entity collection with the updated one
                    cadImage.Entities = updatedEntities;

                    // Save the modified CAD file preserving editable objects
                    string outputPath = "output.dxf";
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath);
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}