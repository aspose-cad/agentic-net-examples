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

namespace AsposeCadPolylineInsertExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input and external DWG file paths
                string mainInputPath = "input.dwg";
                string externalInputPath = "external.dwg";
                string outputPath = "output.dwg";

                // Verify that source files exist
                if (!File.Exists(mainInputPath))
                {
                    Console.Error.WriteLine($"Main input file not found: {mainInputPath}");
                    return;
                }

                if (!File.Exists(externalInputPath))
                {
                    Console.Error.WriteLine($"External DWG file not found: {externalInputPath}");
                    return;
                }

                // Load the main DWG drawing
                using (CadImage mainImage = (CadImage)Image.Load(mainInputPath))
                {
                    // Load the external DWG drawing
                    using (CadImage externalImage = (CadImage)Image.Load(externalInputPath))
                    {
                        // Create a 2D lightweight polyline (closed)
                        CadLwPolyline polyline = new CadLwPolyline();
                        polyline.Coordinates.Add(new Cad2DPoint(0, 0));
                        polyline.Coordinates.Add(new Cad2DPoint(100, 0));
                        polyline.Coordinates.Add(new Cad2DPoint(100, 100));
                        polyline.Coordinates.Add(new Cad2DPoint(0, 100));
                        polyline.Flag = CadLwPolylineFlag.Closed;
                        polyline.PointCount = polyline.Coordinates.Count;

                        // Append the polyline to the main drawing entities
                        mainImage.Entities = mainImage.Entities
                            .Concat(new CadEntityBase[] { polyline })
                            .ToArray();

                        // Append all entities from the external drawing to the main drawing
                        mainImage.Entities = mainImage.Entities
                            .Concat(externalImage.Entities)
                            .ToArray();

                        // Prepare DWG save options (convert geometry to lines for compatibility)
                        DwgOptions dwgOptions = new DwgOptions
                        {
                            OutputMode = CadOutputMode.Convert
                        };

                        // Save the combined drawing
                        mainImage.Save(outputPath, dwgOptions);
                        Console.WriteLine($"Combined drawing saved to: {outputPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}