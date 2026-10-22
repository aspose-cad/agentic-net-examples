using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input CAD file (DWG/DXF) path
                    string inputPath = "sample.dwg";

            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                    string outputPath = "output.pdf";

                    // Ensure the output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Load the CAD file using Aspose.CAD
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD‑specific members
                        CadImage cadImage = (CadImage)image;

                        // Enumerate layers and display basic information
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            bool isOff = layer.ColorId < 0;
                            bool isFrozen = (layer.Flags & 1) != 0;
                            bool isLocked = (layer.Flags & 16) != 0;

                            Console.WriteLine($"Layer: {layer.Name}");
                            Console.WriteLine($"  ColorId : {layer.ColorId}");
                            Console.WriteLine($"  Off     : {isOff}");
                            Console.WriteLine($"  Frozen  : {isFrozen}");
                            Console.WriteLine($"  Locked  : {isLocked}");
                        }

                        // Configure rasterization options for PDF output
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.PageWidth = 1600;   // Width in pixels
                        rasterOptions.PageHeight = 1200;  // Height in pixels

                        // If you need to export only specific layers, assign their names:
                        // rasterOptions.Layers = new string[] { "Layer1", "Layer2" };

                        // Create PDF options and assign the rasterization settings
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the CAD drawing as a PDF file
                        cadImage.Save(outputPath, pdfOptions);
                    }

                    Console.WriteLine("Conversion completed.");
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}