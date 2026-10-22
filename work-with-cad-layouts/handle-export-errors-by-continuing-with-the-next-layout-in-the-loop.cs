using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "sample.dwg";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file using Aspose.CAD
            using (Image image = Image.Load(inputPath))
            {
                // Cast the loaded image to CadImage for CAD-specific options
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Configure rasterization options (e.g., select a layout)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example: select the "Model" layout if it exists
                    Layouts = new string[] { "Model" },
                    // Set page size for the output PDF
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // Optional: set background color, line weight, etc.
                    // BackgroundColor = System.Drawing.Color.White
                };

                // Set up PDF export options using the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as a PDF file
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"CAD file successfully exported to PDF: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}