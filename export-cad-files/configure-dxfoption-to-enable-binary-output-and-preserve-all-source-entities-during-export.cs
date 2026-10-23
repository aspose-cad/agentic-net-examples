using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input DWG/DXF file path
            string inputPath = @"C:\Input\sample.dwg";
            // Output DXF file path
            string outputPath = @"C:\Output\sample_converted.dxf";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file does not exist: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Configure DXF export options
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        // Preserve all source entities (editable) during export
                        OutputMode = CadOutputMode.Convert,
                        // Enable binary DXF output
                        DxfFileFormat = Aspose.CAD.FileFormats.Cad.CadConsts.CadFileFormat.Binary
                    };

                    // Save as DXF using the configured options
                    cadImage.Save(outputPath, dxfOptions);
                }

                Console.WriteLine($"DXF file successfully saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during export: {ex.Message}");
            }
        }
    }
}