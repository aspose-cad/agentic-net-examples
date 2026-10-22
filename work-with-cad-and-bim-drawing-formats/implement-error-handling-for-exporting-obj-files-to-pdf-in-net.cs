using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Define input OBJ file and output PDF file paths
        string inputPath = "input.obj";
        string outputPath = "output.pdf";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the OBJ file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Configure rasterization options for vector to raster conversion if needed
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example page size; adjust as required
                    PageWidth = 800,
                    PageHeight = 600
                };

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the loaded OBJ as a PDF file
                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"Successfully exported OBJ to PDF: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Output any errors that occur during loading or saving
            Console.Error.WriteLine($"Error during export: {ex.Message}");
        }
    }
}