using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        try
        {
            // Input 3DS file path (replace with actual path)
            string inputPath = "sample.3ds";
            // Output PDF file path
            string outputPath = "output.pdf";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the 3DS file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options to flatten 3D objects onto a 2D page
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Set page size (in points; 1 point = 1/72 inch)
                    PageWidth = 595,   // A4 width
                    PageHeight = 842,  // A4 height
                    // Ensure all layers are rendered
                    Layouts = new string[] { "Model" },
                    // Optional: set background color, line weight, etc.
                };

                // Set PDF export options with the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            // Verify that the PDF was created
            if (File.Exists(outputPath))
            {
                Console.WriteLine($"PDF successfully created: {outputPath}");
            }
            else
            {
                Console.Error.WriteLine("Failed to create PDF output.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}