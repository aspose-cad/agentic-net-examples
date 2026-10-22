using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path (DWG, DWF, DGN, etc.)
            string inputPath = @"C:\CAD\sample.dwg";

            // Output DXF file path
            string outputPath = @"C:\CAD\output\sample_converted.dxf";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
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
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Iterate through entities to demonstrate access to line geometry
                    foreach (CadEntityBase entity in cad.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            // Access line points (color property is not available in the current API)
                            Console.WriteLine($"Line found: Start({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                                              $"End({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                        }
                    }

                    // Save as DXF using default options (preserves original entities)
                    DxfOptions dxfOptions = new DxfOptions();
                    cadImage.Save(outputPath, dxfOptions);
                }

                Console.WriteLine($"DXF file saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}