using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadColladaToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source COLLADA file
                    string inputPath = "input.dae";

            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the resulting PDF file
                    string outputPath = "output.pdf";

                    // Load the COLLADA drawing
                    using (Image loadedImage = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)loadedImage;

                        // Example adjustment: modify rasterization options before PDF export
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            // Set desired page size (in pixels)
                            PageWidth = 1200,
                            PageHeight = 800,

                            // Choose which layout(s) to render; for COLLADA typically "Model"
                            Layouts = new string[] { "Model" },

                            // Optional: control background color, line weight scaling, etc.
                            // BackgroundColor = System.Drawing.Color.White,
                            // LineWeightScaling = 1.0
                        };

                        // Create PDF export options and assign the rasterization settings
                        PdfOptions pdfOptions = new PdfOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Save the adjusted drawing as PDF
                        cadImage.Save(outputPath, pdfOptions);
                    }

                    Console.WriteLine("Export completed successfully.");
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}