using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Paths to the source CAD file and the destination PDF file
        string inputFilePath = "input.dwg";
        string outputFilePath = "output.pdf";

        // Verify that the input file exists
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Input file not found: {inputFilePath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputFilePath))
            {
                // Create PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Optional: control rasterization settings (page size, layout, etc.)
                // pdfOptions.VectorRasterizationOptions = new CadRasterizationOptions
                // {
                //     PageWidth = 800,
                //     PageHeight = 600,
                //     Layout = Aspose.CAD.ImageOptions.CadLayoutType.OnePagePerLayer
                // };

                // Export the CAD drawing to PDF
                cadImage.Save(outputFilePath, pdfOptions);
            }

            Console.WriteLine("CAD file successfully exported to PDF.");
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during the export process
            Console.WriteLine("An error occurred during export: " + ex.Message);
        }
    }
}