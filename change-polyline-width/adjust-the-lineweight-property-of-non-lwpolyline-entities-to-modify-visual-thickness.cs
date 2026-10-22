using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadLineWeightAdjustment
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD drawing (DWG/DXF) from file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate over top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Skip lightweight polylines (LWPOLYLINE)
                        if (entity is CadLwPolyline)
                            continue;

                        // Adjust the line weight (visual thickness) of the entity
                        // The value is in hundredths of a millimeter; 200 = 2 mm
                        entity.LineWeight = 200;
                    }

                    // Save the modified drawing preserving editable CAD objects
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}