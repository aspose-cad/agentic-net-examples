using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadRectangleHatchDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                const string inputPath = "empty.dwg";
                const string outputPath = "output.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file \"{inputPath}\" not found.");
                    return;
                }

                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // 1. Create a rectangle as a closed LWPolyline.
                    var rectangle = new CadLwPolyline
                    {
                        Coordinates = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(0, 0),
                            new Cad2DPoint(200, 0),
                            new Cad2DPoint(200, 150),
                            new Cad2DPoint(0, 150)
                        },
                        Flag = CadLwPolylineFlag.Closed,
                        PointCount = 4
                    };

                    // Add rectangle to entities.
                    cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { rectangle }).ToArray();

                    // 2. Create a simple hatch representation using a 3DFace that covers the rectangle area.
                    var hatchFace = new Cad3DFace
                    {
                        FirstCorner = new Cad3DPoint(0, 0, 0),
                        SecondCorner = new Cad3DPoint(200, 0, 0),
                        ThirdCorner = new Cad3DPoint(200, 150, 0),
                        FourthCorner = new Cad3DPoint(0, 150, 0)
                    };

                    // Add hatch (3DFace) to entities.
                    cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { hatchFace }).ToArray();

                    // 3. Remove entities inside the hatch (for demo, remove the rectangle itself).
                    var remaining = cadImage.Entities.Where(e => e != rectangle).ToArray();
                    cadImage.Entities = remaining;

                    // 4. Save the modified drawing.
                    var saveOptions = new DxfOptions
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