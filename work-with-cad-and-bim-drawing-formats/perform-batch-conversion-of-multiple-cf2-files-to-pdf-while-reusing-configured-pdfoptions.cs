using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace BatchCf2ToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input and output directories
            string inputDirectory = "InputCf2";
            string outputDirectory = "OutputPdf";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Configure PDF export options once and reuse for all files
            PdfOptions pdfOptions = new PdfOptions();
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                // Example rasterization settings; adjust as needed
                PageWidth = 1600,
                PageHeight = 1200,
                // Preserve the original layout (Model) if present
                Layouts = new string[] { "Model" }
            };
            pdfOptions.VectorRasterizationOptions = rasterOptions;

            // Get all CF2 files in the input directory
            string[] cf2Files = Directory.GetFiles(inputDirectory, "*.cf2");

            if (cf2Files.Length == 0)
            {
                Console.WriteLine("No CF2 files found to convert.");
                return;
            }

            // Process each file
            foreach (string cf2Path in cf2Files)
            {
                try
                {
                    // Load the CAD image
                    using (Image cadImage = Image.Load(cf2Path))
                    {
                        // Build output PDF path
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(cf2Path);
                        string pdfPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".pdf");

                        // Save as PDF using the pre‑configured options
                        cadImage.Save(pdfPath, pdfOptions);

                        Console.WriteLine($"Converted: {cf2Path} -> {pdfPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting file '{cf2Path}': {ex.Message}");
                }
            }

            Console.WriteLine("Batch conversion completed.");
        }
    }
}