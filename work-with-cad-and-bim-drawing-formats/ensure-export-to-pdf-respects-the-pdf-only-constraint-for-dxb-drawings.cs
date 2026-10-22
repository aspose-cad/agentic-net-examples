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
                // Path to the source DXB file
                string inputPath = "input.dxb";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the resulting PDF file
                string outputPath = "output.pdf";

                // Load the DXB drawing
                using (Aspose.CAD.Image cadImage = Aspose.CAD.Image.Load(inputPath))
                {
                    // Configure PDF export options
                    Aspose.CAD.ImageOptions.PdfOptions pdfOptions = new Aspose.CAD.ImageOptions.PdfOptions();

                    // Optional: control rasterization details for vector content
                    pdfOptions.VectorRasterizationOptions = new Aspose.CAD.ImageOptions.CadRasterizationOptions()
                    {
                        // Example page dimensions (in pixels)
                        PageWidth = 1200,
                        PageHeight = 800,
                        // Preserve all layouts/layers (adjust as needed)
                        Layouts = new string[] { "Model" }
                    };

                    // Save the drawing as PDF
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