using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadObserverExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input CAD file path
                    string inputPath = "input.dxf";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output image file path
                    string outputPath = "output.png";

                    // Load the CAD image
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Configure rasterization options
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.PageWidth = 1600;               // Width of the output image
                        rasterOptions.PageHeight = 1200;              // Height of the output image
                        rasterOptions.UnitType = UnitType.Point;      // Measurement unit
                        rasterOptions.LineScale = 0.5f;               // Optional line scaling

                        // Adjust the observer point to elevate the view.
                        // The three parameters are rotation angles (degrees) around X, Y, Z axes.
                        // Rotating around the X axis gives an elevated perspective.
                        rasterOptions.ObserverPoint = new ObserverPoint(30f, 0f, 0f);

                        // Set up PNG export options
                        PngOptions pngOptions = new PngOptions();
                        pngOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the rendered image
                        cadImage.Save(outputPath, pngOptions);
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