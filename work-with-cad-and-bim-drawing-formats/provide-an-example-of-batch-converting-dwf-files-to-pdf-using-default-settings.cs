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
            string inputFolder = "InputDwf";
            string outputFolder = "OutputPdf";

            // Verify input directory exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Get all DWF files in the input directory
            string[] dwfFiles = Directory.GetFiles(inputFolder, "*.dwf", SearchOption.TopDirectoryOnly);

            foreach (string dwfPath in dwfFiles)
            {
                // Load the DWF file
                using (Image dwfImage = Image.Load(dwfPath))
                {
                    // Create PDF save options (default settings)
                    PdfOptions pdfOptions = new PdfOptions();

                    // Build output PDF file path
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(dwfPath);
                    string pdfPath = Path.Combine(outputFolder, fileNameWithoutExt + ".pdf");

                    // Save the image as PDF
                    dwfImage.Save(pdfPath, pdfOptions);
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