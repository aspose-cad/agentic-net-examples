using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadFbxToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.fbx";
            string outputPath = "output.pdf";

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

            // Configure load options
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            try
            {
                // Load the FBX drawing
                using (Image cadImage = Image.Load(inputPath, loadOptions))
                {
                    // Set up rasterization options for PDF export
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        PageWidth = 800,
                        PageHeight = 1100
                    };

                    // Assign render result callback
                    rasterOptions.RenderResult = (CadRenderResult result) =>
                    {
                        if (result.IsRenderComplete)
                        {
                            Console.WriteLine("Render completed successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Render completed with failures:");
                            foreach (var failure in result.Failures)
                            {
                                Console.WriteLine($"  Code={failure.RenderCode}, Message={failure.Message}");
                            }
                        }
                    };

                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save as PDF
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"PDF saved to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing the FBX file: " + ex.Message);
                // Output any load errors collected
                foreach (var loadError in loadOptions.Errors)
                {
                    Console.WriteLine($"Load error: Code={loadError.RenderCode}, Message={loadError.Message}");
                }
            }
        }
    }
}