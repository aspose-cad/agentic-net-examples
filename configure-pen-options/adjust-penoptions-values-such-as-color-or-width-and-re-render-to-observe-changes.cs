using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path (adjust as needed)
            string inputPath = "sample.dxf";

            // Output directory
            string outputDir = "output";

            try
            {
                // Validate input file
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load options with error collection
                LoadOptions loadOptions = new LoadOptions
                {
                    IgnoreErrors = true
                };

                // Load CAD image
                using (Image image = Image.Load(inputPath, loadOptions))
                {
                    // Cast to CadImage for CAD-specific operations
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Example modification: change color of all line entities to red (ColorId = 1)
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity.TypeName == CadEntityTypeName.LINE)
                        {
                            entity.ColorId = 1; // Red (indexed color)
                        }
                    }

                    // Save as PDF
                    string pdfPath = Path.Combine(outputDir, "result.pdf");
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            LineScale = 1.0f,
                            PageWidth = 800,
                            PageHeight = 600,
                            PenOptions = new PenOptions
                            {
                                StartCap = LineCap.Flat,
                                EndCap = LineCap.Flat
                            }
                        }
                    };
                    cadImage.Save(pdfPath, pdfOptions);

                    // Save as PNG
                    string pngPath = Path.Combine(outputDir, "result.png");
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            LineScale = 1.0f,
                            PageWidth = 800,
                            PageHeight = 600,
                            PenOptions = new PenOptions
                            {
                                StartCap = LineCap.Flat,
                                EndCap = LineCap.Flat
                            }
                        }
                    };
                    cadImage.Save(pngPath, pngOptions);
                }

                Console.WriteLine("Processing completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}