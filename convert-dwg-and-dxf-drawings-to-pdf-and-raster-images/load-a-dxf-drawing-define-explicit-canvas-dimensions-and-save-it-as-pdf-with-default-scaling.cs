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
                // Path to the source DXF file
                string inputPath = "input.dxf";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the DXF drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Define explicit canvas dimensions for rasterization
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 800;   // canvas width in pixels
                    rasterOptions.PageHeight = 600;  // canvas height in pixels

                    // Create PDF save options and assign the rasterization settings
                    PdfOptions pdfOptions = new PdfOptions();
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the drawing as PDF
                    string outputPath = "output.pdf";
                    cadImage.Save(outputPath, pdfOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}