using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to input and output CAD files
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPath = "output.dxf";

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Iterate over top‑level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Validate that the entity type is LWPOLYLINE
                    if (entity.TypeName == CadEntityTypeName.LWPOLYLINE)
                    {
                        // Safe cast after validation
                        CadLwPolyline lwPolyline = (CadLwPolyline)entity;

                        Console.WriteLine($"Top‑level LWPOLYLINE with {lwPolyline.PointCount} points found.");

                        // TODO: Apply width changes here after validation
                    }
                }

                // Check for LWPOLYLINE entities inside block definitions
                foreach (CadBlockEntity blockEntity in cadImage.BlockEntities.Values)
                {
                    foreach (CadEntityBase blockChild in blockEntity.Entities)
                    {
                        if (blockChild.TypeName == CadEntityTypeName.LWPOLYLINE)
                        {
                            CadLwPolyline lwPolylineInBlock = (CadLwPolyline)blockChild;
                            Console.WriteLine($"LWPOLYLINE inside block \"{blockEntity.Name}\" with {lwPolylineInBlock.PointCount} points found.");

                            // TODO: Apply width changes here after validation
                        }
                    }
                }

                // Save the (potentially modified) drawing as DXF
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, dxfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}