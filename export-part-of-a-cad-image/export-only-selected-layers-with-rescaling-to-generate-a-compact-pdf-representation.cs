using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Input CAD file path
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                string outputPath = "output.pdf";

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific members if needed
                    CadImage cad = (CadImage)cadImage;

                    // Configure rasterization options: select layers and define a smaller page size for rescaling
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 800;   // desired width in pixels
                    rasterOptions.PageHeight = 600;  // desired height in pixels
                    rasterOptions.Layers = new string[] { "Layer1", "Layer2" }; // layers to include
                    // Optional: specify layout(s) to render; default is "Model"
                    rasterOptions.Layouts = new string[] { "Model" };

                    // Set up PDF save options and attach the rasterization settings
                    PdfOptions pdfOptions = new PdfOptions();
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the selected layers as a compact PDF
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