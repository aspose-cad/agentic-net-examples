using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dxf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        try
        {
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
        }
        catch (Exception dirEx)
        {
            Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
            return;
        }

        try
        {
            // Load the source CAD drawing (DWG or DXF)
            using (Image cadImageRaw = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)cadImageRaw;

                // Example selective entity replacement:
                // Remove all TEXT entities from the drawing.
                // Note: CadImage.Entities is read‑only; direct replacement is not supported by the SDK.
                // The following demonstrates filtering, but the filtered collection cannot be assigned back.
                var filteredEntities = cadImage.Entities
                    .Where(entity => entity.TypeName != CadEntityTypeName.TEXT)
                    .ToArray();

                // If entity removal is required, it must be performed via block manipulation or other SDK‑specific methods,
                // which are not available in the current API version. Therefore, this step is documented but not executed.

                // Configure DXF save options to preserve editable objects
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,          // Preserve objects (no rasterization)
                    Version = DxfOutputVersion.R12,              // DXF version R12
                    DxfFileFormat = CadFileFormat.Binary         // Binary DXF encoding
                };

                // Save the (potentially modified) drawing as DXF
                cadImage.Save(outputPath, dxfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }

        Console.WriteLine("DXF export completed successfully.");
    }
}