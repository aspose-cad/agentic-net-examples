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
            // Define input and output directories (replace with actual paths as needed)
            string inputFolderPath = "InputIges";
            string outputFolderPath = "Output";

            // Verify input directory exists
            if (!Directory.Exists(inputFolderPath))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolderPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            // Get all IGES files in the input folder
            string[] igesFiles = Directory.GetFiles(inputFolderPath, "*.iges");
            foreach (string igesFilePath in igesFiles)
            {
                string fileBaseName = Path.GetFileNameWithoutExtension(igesFilePath);

                // Load the IGES file
                using (Image cadImage = Image.Load(igesFilePath))
                {
                    // Convert to PDF
                    string pdfPath = Path.Combine(outputFolderPath, fileBaseName + ".pdf");
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(pdfPath, pdfOptions);

                    // Convert to JPEG
                    string jpegPath = Path.Combine(outputFolderPath, fileBaseName + ".jpg");
                    JpegOptions jpegOptions = new JpegOptions();
                    cadImage.Save(jpegPath, jpegOptions);

                    // Convert to PNG
                    string pngPath = Path.Combine(outputFolderPath, fileBaseName + ".png");
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(pngPath, pngOptions);
                }
            }

            Console.WriteLine("Batch conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}