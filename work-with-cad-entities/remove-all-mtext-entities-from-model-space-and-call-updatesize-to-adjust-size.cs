using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace RemoveMTextExample
{
    class Program
    {
        static void Main()
        {
            const string inputPath = "input.dxf";
            const string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Find the Model space layout
                CadLayout modelLayout = cadImage.Layouts.Values.First(layout => layout.LayoutName == "Model");

                // Remove MTEXT entities - not supported directly as CadImage.Entities is read‑only.
                // The Aspose.CAD API does not provide a method to modify the Entities collection or to call UpdateSize on a layout.
                // Therefore this operation cannot be performed with the current SDK version.

                // Prepare save options with explicit DXF format
                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary // or CadFileFormat.Ascii as required
                };

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the (unmodified) CAD file preserving editable objects
                cadImage.Save(outputPath);
                Console.WriteLine("Processing completed. Note: MTEXT removal and layout size update are not supported by the current Aspose.CAD API.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}