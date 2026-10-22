using Aspose.CAD;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Output directory and file
        string outputDir = "Output";
        string outputPath = Path.Combine(outputDir, "MixedEntities.dwg");

        // Ensure the output directory exists
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        try
        {
            // Create an empty CAD image
            var cadImage = new CadImage();

            // Obtain a layout to own the text entity
            var layout = cadImage.Layouts.Values.First();

            // LINE entity
            var line = new CadLine
            {
                FirstPoint = new Cad3DPoint(0, 0, 0),
                SecondPoint = new Cad3DPoint(100, 0, 0)
            };

            // ARC entity (center at (50,50,0), radius 30, from 0 to 180 degrees)
            var arc = new CadArc
            {
                CenterPoint = new Cad3DPoint(50, 50, 0),
                Radius = 30,
                StartAngle = 0,
                EndAngle = Math.PI // 180 degrees in radians
            };

            // TEXT entity
            var text = new CadText
            {
                DefaultValue = "Hello Aspose.CAD",
                FirstAlignment = new Cad3DPoint(10, 10, 0),
                TextHeight = 5,
                TextRotation = 0,
                SoftOwner = layout.BlockTableRecordHandle
            };

            // Add the entities to the image
            cadImage.Entities = cadImage.Entities
                .Concat(new CadEntityBase[] { line, arc, text })
                .ToArray();

            // Save as DWG using conversion mode
            cadImage.Save(outputPath, new DxfOptions { OutputMode = CadOutputMode.Convert });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}