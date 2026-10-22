using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadConversionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file (DWG, DXF, etc.)
                string inputFile = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputFile))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFile}");
                    return;
                }

                // Desired output file
                string outputFile = "output.pdf";

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputFile))
                {
                    // Prepare PDF save options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Configure rasterization options if needed (page size, layout, etc.)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600
                        // Additional options such as background color, layouts, etc., can be set here.
                    };

                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the drawing as PDF
                    cadImage.Save(outputFile, pdfOptions);
                }

                Console.WriteLine("Conversion completed: " + outputFile);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}