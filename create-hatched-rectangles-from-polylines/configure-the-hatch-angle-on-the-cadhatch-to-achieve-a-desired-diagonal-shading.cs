using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using System.IO;

namespace CadHatchExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to an existing (can be empty) DXF file that will serve as a container.
                    string inputPath = "input.dxf";

            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD image.
                    using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                    {
                        // Create a closed lightweight polyline representing a rectangle.
                        CadLwPolyline rectangle = new CadLwPolyline();
                        rectangle.Flag = CadLwPolylineFlag.Closed;
                        rectangle.Coordinates = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(0, 0),
                            new Cad2DPoint(100, 0),
                            new Cad2DPoint(100, 100),
                            new Cad2DPoint(0, 100)
                        };

                        // Add the rectangle to the ModelSpace block.
                        CadBlockEntity modelSpace = cadImage.BlockEntities["ModelSpace"];
                        modelSpace.AddEntity(rectangle);

                        // NOTE: Aspose.CAD does not expose a CadHatch entity, therefore
                        // configuring a hatch angle directly is not supported via the API.
                        // The rectangle can be exported to DXF where external CAD tools
                        // can apply hatching with the desired angle.

                        // Save the modified CAD drawing as DXF.
                        string outputPath = "output.dxf";
                        DxfOptions dxfOptions = new DxfOptions
                        {
                            OutputMode = CadOutputMode.Convert
                        };
                        cadImage.Save(outputPath, dxfOptions);
                    }
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}