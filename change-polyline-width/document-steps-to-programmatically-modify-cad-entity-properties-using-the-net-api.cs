using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

namespace CadEntityModificationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Find the first lightweight polyline (LwPolyline) in the drawing
                    CadLwPolyline polyline = cadImage.Entities
                        .OfType<CadLwPolyline>()
                        .FirstOrDefault();

                    if (polyline != null)
                    {
                        // Set the polyline to be closed
                        polyline.Flag = CadLwPolylineFlag.Closed;

                        // Offset each vertex by (10, 10) units
                        for (int i = 0; i < polyline.Coordinates.Count; i++)
                        {
                            Cad2DPoint original = polyline.Coordinates[i];
                            Cad2DPoint shifted = new Cad2DPoint(original.X + 10.0, original.Y + 10.0);
                            polyline.Coordinates[i] = shifted;
                        }
                    }

                    // Save the modified drawing as DXF, preserving editable objects
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