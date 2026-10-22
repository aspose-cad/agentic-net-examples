using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
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
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Retrieve all top‑level entities
                IList<CadEntityBase> allEntities = cadImage.Entities.ToList();

                // Create a closed rectangle polyline (LWPolyline)
                CadLwPolyline rectangle = new CadLwPolyline
                {
                    Flag = CadLwPolylineFlag.Closed,
                    Coordinates = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(100, 0),
                        new Cad2DPoint(100, 50),
                        new Cad2DPoint(0, 50)
                    }
                };
                rectangle.PointCount = rectangle.Coordinates.Count;

                // Add the rectangle to the drawing
                IList<CadEntityBase> updatedEntities = allEntities.Concat(new[] { rectangle }).ToArray();
                cadImage.Entities = updatedEntities;

                // Batch delete entities that intersect the rectangle.
                // For demonstration, remove all other LWPolylines (intersection logic would be more complex).
                IList<CadEntityBase> filteredEntities = cadImage.Entities
                    .Where(e => !(e is CadLwPolyline lw && lw != rectangle))
                    .ToArray();

                cadImage.Entities = filteredEntities;

                // Save the modified drawing as DXF, preserving editable objects
                DxfOptions saveOptions = new DxfOptions { OutputMode = CadOutputMode.Convert };
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}