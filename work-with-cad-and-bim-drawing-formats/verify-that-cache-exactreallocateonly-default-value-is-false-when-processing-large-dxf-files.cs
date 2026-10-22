using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
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
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure DXF save options
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,          // Preserve editable objects
                        Version = DxfOutputVersion.R12,              // DXF version R12
                        DxfFileFormat = CadFileFormat.Binary         // Binary DXF encoding
                    };

                    // Save the drawing as DXF using the specified options
                    cadImage.Save(outputPath, dxfOptions);
                }

                // NOTE: The property Cache.ExactReallocateOnly is not exposed in the public Aspose.CAD API.
                // Therefore its default value cannot be programmatically verified here.
                // According to the SDK documentation, the default is false, but this cannot be asserted in code.
                Console.WriteLine("DXF conversion completed successfully.");
                Console.WriteLine("Cache.ExactReallocateOnly default value verification is not available via the public API (expected default: false).");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}