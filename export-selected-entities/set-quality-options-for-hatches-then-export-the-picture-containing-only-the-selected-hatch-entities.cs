using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Define input and output paths
        string inputPath = "input.dxf";
        string outputPath = "selected_hatches.png";

        // Ensure a minimal DXF file exists (placeholder creation)
        if (!File.Exists(inputPath))
        {
            string dxfContent = @"0
SECTION
2
ENTITIES
0
HATCH
8
0
62
7
10
0
20
0
30
0
70
1
71
1
91
4
92
0
93
0
0
ENDSEC
0
EOF";
            File.WriteAllText(inputPath, dxfContent);
        }

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Select only hatch entities
                var selectedHatches = cadImage.Entities
                    .OfType<CadHatch>()
                    .Cast<CadEntityBase>()
                    .ToArray();

                // Replace the image's entities with only the selected hatches
                // (Entities property is assignable in this SDK version)
                cadImage.Entities = selectedHatches;

                // Update drawing size after entity modification
                cadImage.UpdateSize(true);

                // Configure rasterization quality (high quality for hatches)
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = cadImage.Size.Width,
                    PageHeight = cadImage.Size.Height,
                    Quality = new RasterizationQuality
                    {
                        Hatch = RasterizationQualityValue.High,
                        Text = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High
                    }
                };

                // Set PNG export options with the rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image containing only the selected hatches
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Export completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}