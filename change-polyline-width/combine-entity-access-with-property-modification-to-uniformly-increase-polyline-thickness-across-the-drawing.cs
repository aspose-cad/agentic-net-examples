using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

namespace CadPolylineThicknessModifier
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input and output file paths (replace with actual paths)
                string inputFilePath = "input.dwg";
                string outputFilePath = "output.dxf";

                // Verify input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputFilePath))
                {
                    // Cast the generic Image to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Iterate over all entities in the drawing
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Handle 2D lightweight polylines
                        if (entity is CadLwPolyline lwPolyline)
                        {
                            // Increase the line weight uniformly (example: add 10 units)
                            lwPolyline.LineWeight = (short)(lwPolyline.LineWeight + 10);
                        }
                        // Handle 3D polylines
                        else if (entity is CadPolyline polyline)
                        {
                            // Increase the line weight uniformly (example: add 10 units)
                            polyline.LineWeight = (short)(polyline.LineWeight + 10);
                        }
                    }

                    // Save the modified drawing to DXF format
                    DxfOptions dxfOptions = new DxfOptions();
                    cadImage.Save(outputFilePath, dxfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}