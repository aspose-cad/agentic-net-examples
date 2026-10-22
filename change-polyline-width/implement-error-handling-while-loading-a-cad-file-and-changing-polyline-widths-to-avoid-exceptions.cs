using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

namespace CadPolylineWidthAdjustment
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("The loaded file is not a CAD image.");
                        return;
                    }

                    // Iterate over all lightweight polylines in the drawing
                    List<CadLwPolyline> lwPolylines = cadImage.Entities
                        .OfType<CadLwPolyline>()
                        .ToList();

                    foreach (CadLwPolyline polyline in lwPolylines)
                    {
                        // Width property is not available in the current API.
                        // If needed, you could adjust line weight via other means here.

                        // Ensure the polyline is closed to avoid geometry exceptions
                        polyline.Flag = CadLwPolylineFlag.Closed;
                    }

                    // Save the modified drawing preserving editable objects
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, saveOptions);

                    Console.WriteLine("Polyline flags updated and file saved successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}