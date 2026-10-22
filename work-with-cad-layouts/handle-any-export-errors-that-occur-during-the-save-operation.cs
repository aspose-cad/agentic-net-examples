using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
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

        try
        {
            // Load the CAD file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options (optional, can control layout, layers, page size, etc.)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example: set the page size to A4
                    PageWidth = 842,
                    PageHeight = 595,
                    // You can specify layouts or layers here if needed, e.g.:
                    // Layouts = new string[] { "Model" },
                    // Layers = new string[] { "0" }
                };

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"File successfully saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during loading or saving
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}