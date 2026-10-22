using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string outputPath = "output.dxf";

        // Validate input file existence
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        var outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD drawing
            using (var cadImage = (CadImage)Image.Load(inputPath))
            {
                // NOTE: The Aspose.CAD API does not expose a LineWeight property on CadEntityBase
                // or on layer objects. Therefore, setting a uniform line weight for a layer's
                // entities cannot be performed via this SDK. The following placeholder demonstrates
                // where such logic would be placed if the feature were available.

                // const string targetLayerName = "MyLayer";
                // const short desiredLineWeight = 25; // hundredths of a millimeter
                // foreach (var entity in cadImage.Entities)
                // {
                //     if (entity.LayerName.Equals(targetLayerName, StringComparison.OrdinalIgnoreCase))
                //     {
                //         // entity.LineWeight = desiredLineWeight; // Not supported
                //     }
                // }

                // Save the drawing as binary DXF, preserving editable objects
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = Aspose.CAD.FileFormats.Cad.CadConsts.CadFileFormat.Binary
                };

                cadImage.Save(outputPath, dxfOptions);
                Console.WriteLine($"Saved output DXF to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}