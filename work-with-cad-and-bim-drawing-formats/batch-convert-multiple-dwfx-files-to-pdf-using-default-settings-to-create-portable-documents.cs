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
            // Placeholder paths – replace with actual directories
            string inputDirectoryPath = @"C:\InputDWFxFiles";
            string outputDirectoryPath = @"C:\OutputPdfFiles";

            if (!Directory.Exists(inputDirectoryPath))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectoryPath}");
                return;
            }

            if (!Directory.Exists(outputDirectoryPath))
            {
                Directory.CreateDirectory(outputDirectoryPath);
            }

            string[] dwfxFiles = Directory.GetFiles(inputDirectoryPath, "*.dwfx", SearchOption.TopDirectoryOnly);
            if (dwfxFiles.Length == 0)
            {
                Console.WriteLine("No DWFx files found to convert.");
                return;
            }

            foreach (string dwfxFilePath in dwfxFiles)
            {
                // Load the DWFx file
                using (Image cadImage = Image.Load(dwfxFilePath))
                {
                    // Prepare PDF save options (default settings)
                    PdfOptions pdfOptions = new PdfOptions();

                    // Determine output PDF file path
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(dwfxFilePath);
                    string pdfFilePath = Path.Combine(outputDirectoryPath, fileNameWithoutExtension + ".pdf");

                    // Save as PDF
                    cadImage.Save(pdfFilePath, pdfOptions);
                    Console.WriteLine($"Converted: {dwfxFilePath} -> {pdfFilePath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}