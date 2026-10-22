using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.Primitives;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string? outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure rasterization options and set the observer (camera) point
                CadRasterizationOptions rasterizationOptions = new CadRasterizationOptions
                {
                    // ObserverPoint expects rotation angles (in degrees). Using placeholder values.
                    ObserverPoint = new ObserverPoint(0f, 0f, 0f)
                };

                // Iterate through entities and adjust hatch pattern origins
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadHatch hatch && hatch.PatternDefinitions != null)
                    {
                        foreach (var patternData in hatch.PatternDefinitions)
                        {
                            if (patternData != null && patternData.LineBasePoint != null)
                            {
                                // Shift the pattern origin by 10 units in X and Y
                                var originalBase = patternData.LineBasePoint;
                                patternData.LineBasePoint = new Cad2DPoint(
                                    originalBase.X + 10,
                                    originalBase.Y + 10);
                            }
                        }
                    }
                }

                // Save the modified drawing using the rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };

                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}