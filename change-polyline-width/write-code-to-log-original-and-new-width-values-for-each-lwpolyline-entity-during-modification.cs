using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadLwPolylineWidthLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input and output file paths
                string inputPath = "input.dwg";
                string outputPath = "output.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Retrieve all LWPOLYLINE entities from the top‑level drawing entities
                    List<CadLwPolyline> lwPolylines = cadImage.Entities
                        .OfType<CadLwPolyline>()
                        .ToList();

                    // Iterate through each LWPOLYLINE, log original width, modify, and log new width
                    foreach (CadLwPolyline polyline in lwPolylines)
                    {
                        // Use ConstantWidth property (the DXF constant width for LWPOLYLINE)
                        double originalWidth = polyline.ConstantWidth;
                        double newWidth = originalWidth * 1.1; // Increase width by 10%

                        // Apply the new width
                        polyline.ConstantWidth = newWidth;

                        // Log the change
                        Console.WriteLine($"LWPOLYLINE - Original Width: {originalWidth}, New Width: {newWidth}");
                    }

                    // Save the modified drawing preserving editable objects
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}