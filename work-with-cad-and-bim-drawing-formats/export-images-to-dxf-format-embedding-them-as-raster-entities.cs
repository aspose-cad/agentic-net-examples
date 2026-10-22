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
            try
            {
                // Path to the source CAD file (DWG, DGN, etc.)
                string sourcePath = "source.dwg";

                // Path for the output DXF file
                string outputPath = "output.dxf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the source CAD image
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Configure DXF save options to preserve objects (Convert mode)
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        Version = DxfOutputVersion.R12,
                        DxfFileFormat = CadFileFormat.Ascii
                    };

                    // Save the image as DXF, embedding any raster data as raster entities
                    cadImage.Save(outputPath, dxfOptions);
                }

                Console.WriteLine("DXF file has been created at: " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}