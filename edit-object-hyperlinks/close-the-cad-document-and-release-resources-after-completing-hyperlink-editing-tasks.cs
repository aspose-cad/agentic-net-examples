using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string sourceFile = "sample.dwg";

        if (!File.Exists(sourceFile))
        {
            Console.Error.WriteLine($"Source file '{sourceFile}' does not exist.");
            return;
        }

        try
        {
            // Load the CAD image
            using (Image cadImage = Image.Load(sourceFile))
            {
                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Path for the output PDF file
                string outputFile = "output.pdf";

                // Save the CAD drawing as PDF
                cadImage.Save(outputFile, pdfOptions);
            } // cadImage disposed here, releasing resources
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}