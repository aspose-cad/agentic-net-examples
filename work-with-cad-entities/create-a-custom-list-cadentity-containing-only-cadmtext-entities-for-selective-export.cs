using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD;
using System;
using System.Collections.Generic;
using System.Linq;
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
                // Load the source CAD file
                string inputPath = "input.dxf";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        CadImage sourceImage = (CadImage)Aspose.CAD.Image.Load(inputPath);

                // Select the first layout (adjust if needed)
                CadLayout layout = sourceImage.Layouts.Values.First();

                // Collect only CadMText entities from the layout
                List<CadEntityBase> mtextList = new List<CadEntityBase>();
                foreach (CadEntityBase entity in layout.Entities)
                {
                    if (entity.TypeName == CadEntityTypeName.MTEXT)
                    {
                        CadMText mtext = (CadMText)entity;
                        // Assign SoftOwner to the layout's block table record handle
                        mtext.SoftOwner = layout.BlockTableRecordHandle;
                        mtextList.Add(mtext);
                    }
                }

                // Create a new CAD image containing only the collected MText entities
                CadImage exportImage = new CadImage
                {
                    Entities = mtextList.ToArray(),
                    DefaultFont = sourceImage.DefaultFont
                };

                // Save the new CAD file preserving editable objects
                string outputPath = "output.dxf";
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                exportImage.Save(outputPath);
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}