using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace OptimizeHatchOrigin
{
    class Program
    {
        static void Main(string[] args)
        {
            // Expect two arguments: input CAD file path and output PDF path
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Usage: OptimizeHatchOrigin <input-cad-file> <output-pdf>");
                return;
            }

            string inputPath = args[0];
            string outputPath = args[1];

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
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
                // Load CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Iterate over top‑level entities and adjust hatch origins
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Use runtime type name to avoid direct dependency on CadHatch class name
                        if (entity.GetType().Name == "CadHatch")
                        {
                            dynamic hatch = entity;

                            // Set hatch pattern origin to (0,0,0) for better performance
                            hatch.PatternOrigin = new Cad3DPoint(0f, 0f, 0f);

                            // Additionally, modify the pattern data if available
                            if (hatch.PatternData != null)
                            {
                                hatch.PatternData.Origin = new Cad3DPoint(0f, 0f, 0f);
                            }
                        }
                    }

                    // Prepare PDF export options with rasterization settings
                    PdfOptions pdfOptions = new PdfOptions();
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1200f,
                        PageHeight = 1200f,
                        LineScale = 1.0f,
                        AutomaticLayoutsScaling = false,
                        Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                    };
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the modified CAD as PDF
                    image.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully saved optimized PDF to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}