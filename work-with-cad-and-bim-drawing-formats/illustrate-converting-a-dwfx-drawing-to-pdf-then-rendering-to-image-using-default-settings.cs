using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output file paths
            string inputPath = "sample.dwf";
            string pdfOutputPath = "output.pdf";
            
            string outputDir = Path.GetDirectoryName(pdfOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
string pngOutputPath = "output.png";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWFx drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Save the drawing as PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfOutputPath, pdfOptions);
                Console.WriteLine($"PDF saved to: {pdfOutputPath}");

                // Render the drawing to a PNG image using default settings
                PngOptions pngOptions = new PngOptions();
                cadImage.Save(pngOutputPath, pngOptions);
                Console.WriteLine($"PNG image saved to: {pngOutputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}