using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AddBackgroundImageExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input DXF file path
            string inputPath = "input.dxf";
            // Output PDF file path
            string outputPath = "output.pdf";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Error: Input file \"{inputPath}\" does not exist.");
                return;
            }

            // Load the DXF file
            CadImage cadImage = null;
            try
            {
                // Aspose.CAD.Image.Load returns an Image; cast to CadImage for DXF handling
                cadImage = (CadImage)Image.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading DXF file: {ex.Message}");
                return;
            }

            // Prepare PDF export options
            PdfOptions pdfOptions = new PdfOptions();

            // If specific rasterization settings are required, configure them here
            // Example: set page size or resolution
            // CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            // {
            //     PageWidth = 800,
            //     PageHeight = 600,
            //     Layouts = new[] { "Model" }
            // };
            // pdfOptions.VectorRasterizationOptions = rasterOptions;

            // Export to PDF with error handling
            try
            {
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"DXF file successfully exported to PDF: \"{outputPath}\"");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during PDF export: {ex.Message}");
            }
            finally
            {
                // Ensure the image is disposed
                cadImage?.Dispose();
            }
        }
    }
}
