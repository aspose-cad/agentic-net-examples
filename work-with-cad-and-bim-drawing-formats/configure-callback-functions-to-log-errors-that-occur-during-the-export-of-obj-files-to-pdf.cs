using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadObjToPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input OBJ file path (replace with actual path or keep placeholder)
            string inputPath = "model.obj";
            // Output PDF file path (replace with actual path or keep placeholder)
            string outputPath = "output.pdf";

            // Guard against missing input file
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

            // Prepare load options with error ignoring
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            Image cadImage = null;
            try
            {
                // Load the OBJ file as a CAD image
                cadImage = Image.Load(inputPath, loadOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load OBJ file: {ex.Message}");
                return;
            }

            // Log any errors that occurred during loading
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                foreach (var loadError in loadOptions.Errors)
                {
                    Console.Error.WriteLine($"Load error - {loadError.RenderCode}: {loadError.Message}");
                }
            }

            // Configure PDF export options with rendering diagnostics callback
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = new CadRasterizationOptions
                {
                    PageWidth = 595,
                    PageHeight = 842,
                    RenderResult = result =>
                    {
                        // Log rendering failures, if any
                        if (result.Failures != null)
                        {
                            foreach (var failure in result.Failures)
                            {
                                Console.Error.WriteLine($"Render failure - {failure.RenderCode}: {failure.Message}");
                            }
                        }
                    }
                }
            };

            try
            {
                // Export the CAD image to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF successfully saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save PDF: {ex.Message}");
            }
            finally
            {
                // Release resources
                cadImage?.Dispose();
            }
        }
    }
}