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
            // Define input and output directories (replace with actual paths)
            string inputDirectory = @"C:\InputIGES";
            string outputDirectory = @"C:\Output";

            // Verify that the input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Get all IGES files in the input directory
            string[] igesFiles = Directory.GetFiles(inputDirectory, "*.iges", SearchOption.TopDirectoryOnly);
            if (igesFiles.Length == 0)
            {
                Console.WriteLine("No IGES files found to process.");
                return;
            }

            foreach (string igesPath in igesFiles)
            {
                // Load the IGES file using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(igesPath))
                {
                    // Prepare base file name without extension
                    string baseFileName = Path.GetFileNameWithoutExtension(igesPath);

                    // Save as PDF
                    string pdfPath = Path.Combine(outputDirectory, baseFileName + ".pdf");
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(pdfPath, pdfOptions);

                    // Save as JPEG
                    string jpegPath = Path.Combine(outputDirectory, baseFileName + ".jpeg");
                    JpegOptions jpegOptions = new JpegOptions();
                    cadImage.Save(jpegPath, jpegOptions);

                    // Save as PNG
                    string pngPath = Path.Combine(outputDirectory, baseFileName + ".png");
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(pngPath, pngOptions);

                    Console.WriteLine($"Processed: {igesPath}");
                }
            }

            Console.WriteLine("All files have been converted successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}