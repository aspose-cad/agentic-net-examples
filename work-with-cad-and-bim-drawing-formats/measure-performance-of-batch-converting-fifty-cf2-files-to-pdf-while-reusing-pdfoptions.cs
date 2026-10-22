using System;
using System.IO;
using System.Diagnostics;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output directories
            string inputFolder = @"C:\InputCF2";
            string outputFolder = @"C:\OutputPDF";

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

            // Get up to 50 CF2 files
            string[] cf2Files = Directory.GetFiles(inputFolder, "*.cf2");
            int filesToProcess = Math.Min(50, cf2Files.Length);
            if (filesToProcess == 0)
            {
                Console.WriteLine("No CF2 files found to process.");
                return;
            }

            // Reuse a single PdfOptions instance for all conversions
            PdfOptions pdfOptions = new PdfOptions
            {
                // Use a concrete implementation of VectorRasterizationOptions
                VectorRasterizationOptions = new CadRasterizationOptions()
            };

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < filesToProcess; i++)
            {
                string cf2Path = cf2Files[i];
                string outputFileName = Path.GetFileNameWithoutExtension(cf2Path) + ".pdf";
                string pdfPath = Path.Combine(outputFolder, outputFileName);

                // Load the CF2 file and save as PDF
                Image cadImage = Image.Load(cf2Path);
                cadImage.Save(pdfPath, pdfOptions);
            }

            stopwatch.Stop();
            Console.WriteLine($"Converted {filesToProcess} CF2 files to PDF in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}