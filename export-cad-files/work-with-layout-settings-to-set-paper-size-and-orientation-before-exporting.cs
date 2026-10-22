using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options for layout, paper size and orientation
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Set the desired paper size (A4) and orientation (landscape)
                    // A4 size: 210mm x 297mm => 595 x 842 points (1 point = 1/72 inch)
                    PageWidth = 842,   // width in points (landscape)
                    PageHeight = 595   // height in points
                };

                // Create PDF save options and attach the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as PDF with the specified layout settings
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Successfully exported to PDF: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}