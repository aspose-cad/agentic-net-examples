using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.stl";
        string outputPath = "output.pdf";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string? outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir))
                Directory.CreateDirectory(outDir);

            // Load options: ignore errors but collect them
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                // Report any load errors
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.Error.WriteLine("Load completed with errors:");
                    foreach (var error in loadOptions.Errors)
                    {
                        Console.Error.WriteLine($" - {error.Message}");
                    }
                }

                // Prepare PDF save options
                PdfOptions pdfOptions = new PdfOptions();

                // Optional rasterization options with render callback
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    Layouts = new[] { "Model" },
                    RenderResult = result =>
                    {
                        if (!result.IsRenderComplete)
                        {
                            Console.Error.WriteLine("Rendering failed with the following issues:");
                            foreach (var failure in result.Failures)
                            {
                                Console.Error.WriteLine($" * {failure}");
                            }
                        }
                    }
                };

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the STL as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"File saved successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An exception occurred: {ex.Message}");
        }
    }
}