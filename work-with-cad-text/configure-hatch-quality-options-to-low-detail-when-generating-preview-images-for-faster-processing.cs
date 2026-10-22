using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.Primitives;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputDxfPath = "output.dxf";
        const string previewPath = "preview.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load an existing CAD drawing (DWG format)
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Iterate over all entities and update text strings
                CadEntityBase[] updatedEntities = cadImage.Entities
                    .Select(entity =>
                    {
                        if (entity is CadText textEntity)
                        {
                            // Prefix each text string with a custom label
                            textEntity.DefaultValue = "Updated: " + textEntity.DefaultValue;
                        }
                        return entity;
                    })
                    .ToArray();

                // Replace the image's entity collection with the modified one
                cadImage.Entities = updatedEntities;

                // Save the modified drawing as DXF while preserving editability
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };
                cadImage.Save(outputDxfPath, dxfOptions);

                // Configure rasterization options with low-quality hatch for faster preview generation
                var rasterOptions = new CadRasterizationOptions
                {
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.Low,
                        Hatch = RasterizationQualityValue.Low,
                        Arc = RasterizationQualityValue.Low
                    }
                };

                // Save a preview image (PNG) using the low-quality rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Ensure output directory exists
                string previewDir = Path.GetDirectoryName(previewPath);
                if (!string.IsNullOrEmpty(previewDir))
                {
                    Directory.CreateDirectory(previewDir);
                }

                cadImage.Save(previewPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }
}