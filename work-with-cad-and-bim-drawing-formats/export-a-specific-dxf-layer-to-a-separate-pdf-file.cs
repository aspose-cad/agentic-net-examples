using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class ExportDxfLayerToPdf
{
    static void Main(string[] args)
    {
        try
        {
            // Validate command‑line arguments.
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: ExportDxfLayerToPdf <source-dxf-path> <layer-name>");
                return;
            }

            string sourcePath = args[0];
            string layerName = args[1];

            // Verify source file exists.
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the DXF file.
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD‑specific members.
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("The provided file is not a supported CAD format.");
                    return;
                }

                // Configure rasterization options to render only the requested layer.
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1600,
                    Layers = new[] { layerName }
                };

                // Configure PDF export options.
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Build output file name.
                string outputPath = Path.ChangeExtension(sourcePath, $"_{layerName}.pdf");

                // Ensure output directory exists.
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the selected layer as PDF.
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Layer \"{layerName}\" exported to PDF: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}