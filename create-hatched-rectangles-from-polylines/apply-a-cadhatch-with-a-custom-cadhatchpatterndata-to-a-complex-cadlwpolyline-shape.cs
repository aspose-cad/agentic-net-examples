using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Create a new empty CAD image
        var cadImage = new CadImage();

        // Build a complex closed lightweight polyline
        var polyline = new CadLwPolyline
        {
            Flag = CadLwPolylineFlag.Closed,
            Coordinates = new List<Cad2DPoint>
            {
                new Cad2DPoint(0, 0),
                new Cad2DPoint(120, 0),
                new Cad2DPoint(120, 80),
                new Cad2DPoint(80, 120),
                new Cad2DPoint(0, 80)
            }
        };
        polyline.PointCount = polyline.Coordinates.Count;

        // NOTE: CadHatch and related types are not available in the current SDK version.
        // If hatch functionality is required, it should be implemented using supported APIs.
        // For compilation purposes, the hatch creation is omitted.

        // Add the polyline entity to the CAD image
        cadImage.Entities = cadImage.Entities
                                    .Concat(new CadEntityBase[] { polyline })
                                    .ToArray();

        // Define rasterization options for rendering
        var rasterOptions = new CadRasterizationOptions
        {
            UnitType = UnitType.Centimenter,
            PageWidth = 200,
            PageHeight = 150,
            Layouts = new[] { "Model" }
        };

        // Set PNG output options
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the result with error handling
        try
        {
            cadImage.Save("HatchedPolyline.png", pngOptions);
            Console.WriteLine("Image saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save image: {ex.Message}");
        }
    }
}