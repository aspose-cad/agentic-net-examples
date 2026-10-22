using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source DWFx drawing.
                string inputPath = "input.dwf";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path where the resulting PDF will be saved.
                string outputPath = "output.pdf";

                // Load the DWFx file. Aspose.CAD.Image.Load returns an Image object that
                // represents the CAD document and provides access to its content.
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Create PDF export options. When no specific settings are changed,
                    // the default options perform a direct conversion, preserving vector
                    // information and using the original drawing dimensions.
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the loaded CAD image to PDF using the default options.
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("DWFx to PDF conversion completed.");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}