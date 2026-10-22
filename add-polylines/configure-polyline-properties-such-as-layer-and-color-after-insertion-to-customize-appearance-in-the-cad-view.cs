using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (var cadImage = (CadImage)Image.Load(inputPath))
            {
                // Create a new lightweight polyline
                var polyline = new CadLwPolyline
                {
                    ColorId = 1, // red
                    Flag = CadLwPolylineFlag.Closed
                };

                // Define polyline vertices
                polyline.Coordinates.Add(new Cad2DPoint(0, 0));
                polyline.Coordinates.Add(new Cad2DPoint(100, 0));
                polyline.Coordinates.Add(new Cad2DPoint(100, 100));
                polyline.Coordinates.Add(new Cad2DPoint(0, 100));

                // Ensure the target layer exists; create if missing
                const string targetLayerName = "MyLayer";
                CadLayerTable targetLayer = null;

                foreach (CadLayerTable layerObj in cadImage.Layers)
                {
                    if (layerObj is CadLayerTable layer && layer.Name == targetLayerName)
                    {
                        targetLayer = layer;
                        break;
                    }
                }

                if (targetLayer == null)
                {
                    targetLayer = new CadLayerTable
                    {
                        Name = targetLayerName,
                        ColorId = 1 // red
                    };
                    cadImage.Layers.Add(targetLayer);
                }

                // Assign the polyline to the target layer
                polyline.LayerName = targetLayer.Name;

                // Add the polyline to the drawing entities
                var entityList = cadImage.Entities.ToList();
                entityList.Add(polyline);
                cadImage.Entities = entityList.ToArray();

                // Prepare rasterization options for PDF export
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    Layouts = new[] { "Model" },
                    Layers = new[] { targetLayer.Name }
                };

                // Set PDF options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}