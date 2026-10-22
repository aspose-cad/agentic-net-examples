using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace ExportSelectedLayersWithRescaling
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input and output file paths
                string inputFile = "input.dwg";
                string outputFile = "output.pdf";

                // Verify input file exists
                if (!File.Exists(inputFile))
                {
                    Console.WriteLine($"Input file not found: {inputFile}");
                    return;
                }

                // Load the CAD drawing
                using (Image loadedImage = Image.Load(inputFile))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)loadedImage;

                    // Select entities that belong to the desired layer
                    CadEntityBase[] selectedEntities = cadImage.Entities
                        .Where(entity => entity.LayerName == "LayerToExport")
                        .ToArray();

                    // Assign the selected entities back to the drawing before export
                    cadImage.Entities = selectedEntities;

                    // Configure rasterization options (including rescaling)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,          // Desired width in pixels
                        PageHeight = 600,         // Desired height in pixels
                        Layouts = new[] { "Model" } // Export the Model layout
                    };

                    // Configure PDF export options and attach rasterization settings
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the selected layers to PDF with the specified scaling
                    cadImage.Save(outputFile, pdfOptions);
                    Console.WriteLine($"Export completed: {outputFile}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}