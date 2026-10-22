using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "template.dxf";
            string outputDir = "output";
            string outputPath = Path.Combine(outputDir, "output.dxf");

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load existing DXF
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Create a 2‑D lightweight polyline (closed square)
                CadLwPolyline polyline = new CadLwPolyline();
                polyline.Coordinates.Add(new Cad2DPoint(0, 0));
                polyline.Coordinates.Add(new Cad2DPoint(100, 0));
                polyline.Coordinates.Add(new Cad2DPoint(100, 100));
                polyline.Coordinates.Add(new Cad2DPoint(0, 100));
                polyline.Flag = CadLwPolylineFlag.Closed;

                // Append the polyline to the drawing
                cadImage.Entities = cadImage.Entities.Concat(new[] { polyline }).ToArray();

                // Ensure output directory exists
                Directory.CreateDirectory(outputDir);

                // Configure DXF export options with R12 version and ASCII format
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii,
                    Version = DxfOutputVersion.R12
                };

                // Save the drawing
                cadImage.Save(outputPath, dxfOptions);
                Console.WriteLine($"DXF saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}