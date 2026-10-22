using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;
using System.IO;

namespace CreateHatchedRectangles
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Load an existing (empty) CAD file. Replace the path with a valid DWG/DXF file.
                    string inputPath = "empty.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        CadImage cadImage = (CadImage)Image.Load(inputPath);

                    // Retrieve the Model Space block where entities are stored.
                    CadBlockEntity modelSpace = cadImage.BlockEntities["*Model_Space"];

                    // Define rectangle coordinates (lower‑left, lower‑right, upper‑right, upper‑left).
                    List<Cad2DPoint[]> rectangles = new List<Cad2DPoint[]>
                    {
                        new Cad2DPoint[]
                        {
                            new Cad2DPoint(0, 0),
                            new Cad2DPoint(100, 0),
                            new Cad2DPoint(100, 50),
                            new Cad2DPoint(0, 50)
                        },
                        new Cad2DPoint[]
                        {
                            new Cad2DPoint(150, 0),
                            new Cad2DPoint(250, 0),
                            new Cad2DPoint(250, 80),
                            new Cad2DPoint(150, 80)
                        },
                        new Cad2DPoint[]
                        {
                            new Cad2DPoint(300, 0),
                            new Cad2DPoint(400, 0),
                            new Cad2DPoint(400, 120),
                            new Cad2DPoint(300, 120)
                        }
                    };

                    // Add each rectangle as a closed lightweight polyline.
                    foreach (Cad2DPoint[] pts in rectangles)
                    {
                        CadLwPolyline polyline = new CadLwPolyline();
                        polyline.Flag = CadLwPolylineFlag.Closed;

                        foreach (Cad2DPoint pt in pts)
                        {
                            polyline.Coordinates.Add(pt);
                        }

                        // Note: Aspose.CAD does not expose a CadHatch entity in the verified API.
                        // Therefore, hatch filling cannot be applied directly. The polylines are added as outlines.

                        modelSpace.AddEntity(polyline);
                    }

                    // Set up rasterization options for vector output (e.g., SVG).
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f,
                        PageWidth = 800,
                        PageHeight = 600,
                        AutomaticLayoutsScaling = true,
                        Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                    };

                    // Save the result as SVG (vector format).
                    SvgOptions svgOptions = new SvgOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string outputPath = "HatchedRectangles.svg";
                    cadImage.Save(outputPath, svgOptions);
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}