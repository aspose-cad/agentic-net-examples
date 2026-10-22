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
            // Define input and output directories
            string inputDirectory = @"C:\InputPltFiles";
            string outputDirectory = @"C:\OutputImages";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Get all PLT files in the input directory
            string[] pltFiles = Directory.GetFiles(inputDirectory, "*.plt", SearchOption.TopDirectoryOnly);

            foreach (string pltFilePath in pltFiles)
            {
                // Load the PLT file
                using (Image cadImage = Image.Load(pltFilePath))
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(pltFilePath);

                    // Convert to PDF
                    string pdfPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".pdf");
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(pdfPath, pdfOptions);

                    // Convert to JPEG
                    string jpegPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".jpg");
                    JpegOptions jpegOptions = new JpegOptions();
                    cadImage.Save(jpegPath, jpegOptions);

                    // Convert to PNG
                    string pngPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".png");
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(pngPath, pngOptions);
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}