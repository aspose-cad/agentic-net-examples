using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace AsposeCadDxfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source CAD file (could be any supported format, e.g., DWG, DXF)
            string sourcePath = "input.dxf";

            // Path for the exported DXF file
            string outputPath = "output.dxf";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            try
            {
                // Load the source CAD image
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Create DXF export options
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        // Preserve objects (no geometry conversion)
                        OutputMode = CadOutputMode.Convert,
                        // Use binary DXF encoding
                        DxfFileFormat = CadFileFormat.Binary,
                        // DXF version R12 (the only supported version)
                        Version = DxfOutputVersion.R12
                    };

                    // NOTE: Embedding a separate 3D file as an auxiliary object in the DXF
                    // is not supported by the current Aspose.CAD API. The SDK does not expose
                    // functionality to attach external 3D files as auxiliary objects during export.

                    // Save the image as DXF with the specified options
                    cadImage.Save(outputPath, dxfOptions);
                }

                Console.WriteLine("DXF export completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during processing: {ex.Message}");
            }
        }
    }
}