using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

namespace CadPolylineWidthModifier
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Path for the modified output file
                string outputPath = "output.dxf";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate over top‑level entities and modify polyline line weights
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Handle 2‑D lightweight polylines
                        if (entity is CadLwPolyline lwPolyline)
                        {
                            // Set line weight (value is in 1/100 mm; 50 = 0.5 mm)
                            lwPolyline.LineWeight = 50;
                        }
                        // Handle native 2‑D/3‑D polylines
                        else if (entity is CadPolyline cadPolyline)
                        {
                            cadPolyline.LineWeight = 50;
                        }
                    }

                    // Prepare save options for DXF output while preserving editable objects
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };

                    // Save the modified drawing
                    cadImage.Save(outputPath, saveOptions);
                }

                Console.WriteLine("Polyline line weights have been updated and saved to: " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}