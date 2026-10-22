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
            // Define input and output directories (adjust paths as needed)
            string inputFolder = "InputDrawings";
            string outputFolder = "OutputPdfs";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Get all DXB files in the input folder
            string[] drawingFiles = Directory.GetFiles(inputFolder, "*.dxb");
            if (drawingFiles.Length == 0)
            {
                Console.WriteLine("No DXB files found in the input folder.");
                return;
            }

            // Process each DXB file
            foreach (string drawingPath in drawingFiles)
            {
                // Build output PDF file path
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(drawingPath);
                string outputPath = Path.Combine(outputFolder, fileNameWithoutExt + ".pdf");

                // Load the CAD drawing
                using (Image cadImage = Image.Load(drawingPath))
                {
                    // Set PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Converted '{drawingPath}' to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            // Print any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}