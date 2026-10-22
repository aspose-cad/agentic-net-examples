using Aspose.CAD;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class CadToPdfExporter
{
    static void Main(string[] args)
    {
        // Expect input CAD file path and output PDF file path
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: CadToPdfExporter <input.cad> <output.pdf>");
            return;
        }

        string inputPath = args[0];
        string outputPath = args[1];

        // Validate input file existence
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file does not exist: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (Aspose.CAD.Image image = Aspose.CAD.Image.Load(inputPath))
            {
                // Cast to CadImage to access layout information
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image as CadImage.");
                    return;
                }

                // Retrieve the first layout (or handle multiple layouts as needed)
                var layout = cadImage.Layouts.Values.FirstOrDefault();
                if (layout == null)
                {
                    Console.WriteLine("No layout found in the CAD file.");
                    return;
                }

                // Compute layout dimensions using extents (width = MaxX - MinX, height = MaxY - MinY)
                float width = (float)(layout.MaxExtents.X - layout.MinExtents.X);
                float height = (float)(layout.MaxExtents.Y - layout.MinExtents.Y);

                // Report layout dimensions for verification
                Console.WriteLine($"Layout dimensions: Width = {width} cm, Height = {height} cm");

                // Configure rasterization options with custom quality settings
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,          // Correct enum spelling
                    AutomaticLayoutsScaling = false,         // Preserve original layout scale
                    LineScale = 1f,                           // Default line thickness
                    PageWidth = width,                        // Match original layout width
                    PageHeight = height                       // Match original layout height
                    // Margins can be set here if required
                };

                // Create PDF export options and assign rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF successfully saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}