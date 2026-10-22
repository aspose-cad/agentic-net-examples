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
            // Path to the source STP file (replace with actual path)
            string inputPath = "sample.stp";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the STP file using Aspose.CAD.Image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Output basic information about the loaded image
                Console.WriteLine($"Loaded STP file. Unit type: {cadImage.UnitType}");

                // Example: save the loaded drawing as PDF to confirm successful loading
                string outputPath = "output.pdf";
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Saved PDF to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}