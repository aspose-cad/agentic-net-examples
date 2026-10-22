using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file – replace with an existing file path.
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory.
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "selected_layers_rescaled.pdf");

            // Load the CAD image.
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members.
                CadImage cadImage = (CadImage)image;

                // Configure rasterization options.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Set measurement unit.
                    UnitType = UnitType.Centimenter,

                    // Define page size (A4 in centimeters).
                    PageWidth = 210,
                    PageHeight = 297,

                    // Disable automatic layout scaling to keep explicit dimensions.
                    AutomaticLayoutsScaling = false,

                    // Adjust line thickness scaling if needed.
                    LineScale = 0.5f,

                    // Specify which layers to export.
                    Layers = new string[] { "Layer1", "Layer2" }
                };

                // Set margins (left, right, top, bottom) in the same unit.
                rasterOptions.Margins = new Margins();
                rasterOptions.Margins.Left = 5;
                rasterOptions.Margins.Right = 5;
                rasterOptions.Margins.Top = 5;
                rasterOptions.Margins.Bottom = 5;

                // Apply UpdateSize after any modifications to guarantee correct scaling.
                cadImage.UpdateSize();

                // Configure PDF export options and attach rasterization settings.
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the selected layers with the defined scaling.
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Export completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}