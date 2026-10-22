using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadSvgExportLogging
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Paths for input CAD file and output SVG file
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.svg";

                    // Load the CAD image
                    using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                    {
                        // Configure rasterization options
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.UnitType = UnitType.Centimenter; // SDK spelling
                        rasterOptions.LineScale = 1.0f;
                        rasterOptions.PageWidth = 800;   // optional page width
                        rasterOptions.PageHeight = 600;  // optional page height
                        rasterOptions.Margins = new Margins()
                        {
                            Left = 10,
                            Right = 10,
                            Top = 10,
                            Bottom = 10
                        };

                        // Set up SVG export options
                        SvgOptions svgOptions = new SvgOptions();
                        svgOptions.VectorRasterizationOptions = rasterOptions;

                        // Log start of export
                        Console.WriteLine($"Starting SVG export: '{inputPath}' -> '{outputPath}'");

                        // Perform the export
                        cadImage.Save(outputPath, svgOptions);

                        // Log completion
                        Console.WriteLine("SVG export completed successfully.");
                    }
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}