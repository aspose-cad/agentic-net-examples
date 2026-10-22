using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class ExportSelectedLayersAndCheckSize
{
    static void Main()
    {
        // Input CAD file path
        string inputPath = @"C:\Data\sample.dwg";

        // Output PDF file path
        string outputPath = @"C:\Data\output.pdf";

        // Layers to export
        string[] selectedLayers = new[] { "Layer1", "Layer2" };

        // Expected dimensions (in centimeters, because UnitType is Centimenter)
        float expectedWidth = 210f;   // example width
        float expectedHeight = 297f;  // example height

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure rasterization options for selected layers
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 0.5f,
                    AutomaticLayoutsScaling = true,
                    Layouts = selectedLayers,
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                };

                // Set up PDF export options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export selected layers to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Exported PDF saved to: {outputPath}");

                // Retrieve layout size (using the first layout that matches the selected ones)
                foreach (var kvp in cadImage.Layouts)
                {
                    string layoutName = kvp.Key;
                    CadLayout layout = kvp.Value;

                    if (Array.IndexOf(selectedLayers, layoutName) >= 0)
                    {
                        float width = (float)(layout.MaxExtents.X - layout.MinExtents.X);
                        float height = (float)(layout.MaxExtents.Y - layout.MinExtents.Y);

                        Console.WriteLine($"Layout \"{layoutName}\" size: {width:F2} cm × {height:F2} cm");

                        bool widthMatches = Math.Abs(width - expectedWidth) < 0.01f;
                        bool heightMatches = Math.Abs(height - expectedHeight) < 0.01f;

                        if (widthMatches && heightMatches)
                        {
                            Console.WriteLine("Layout dimensions match the expected size.");
                        }
                        else
                        {
                            Console.WriteLine("Layout dimensions do NOT match the expected size.");
                        }

                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during processing: {ex.Message}");
        }
    }
}