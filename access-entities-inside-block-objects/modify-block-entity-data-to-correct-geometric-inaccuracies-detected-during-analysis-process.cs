using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadBlockModificationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DWG/DXF file path
                string inputPath = @"C:\CAD\input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Output DWG/DXF file path
                string outputPath = @"C:\CAD\Modified\output.dwg";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Define an offset to correct geometric inaccuracies
                    double offsetX = 10.0;
                    double offsetY = 5.0;

                    // Iterate through all block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        // Iterate through entities inside the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            // Adjust line entities as an example correction
                            if (entity is CadLine line)
                            {
                                // Create new points with the applied offset
                                var newFirst = new Cad3DPoint(
                                    line.FirstPoint.X + offsetX,
                                    line.FirstPoint.Y + offsetY,
                                    line.FirstPoint.Z);

                                var newSecond = new Cad3DPoint(
                                    line.SecondPoint.X + offsetX,
                                    line.SecondPoint.Y + offsetY,
                                    line.SecondPoint.Z);

                                // Apply the corrected points
                                line.FirstPoint = newFirst;
                                line.SecondPoint = newSecond;
                            }

                            // Additional entity type corrections can be added here
                        }
                    }

                    // Save the modified CAD drawing
                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}