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
            // Define input CAD file path (replace with actual file path)
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory and file path
            string outputDirectory = "output";
            Directory.CreateDirectory(outputDirectory);
            string outputPath = Path.Combine(outputDirectory, "output.pdf");

            // Load the CAD image
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options with black drawing color
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                DrawColor = Aspose.CAD.Color.Black
            };

            // Set up PDF save options and assign rasterization options
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the CAD drawing as PDF
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine($"PDF file saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}