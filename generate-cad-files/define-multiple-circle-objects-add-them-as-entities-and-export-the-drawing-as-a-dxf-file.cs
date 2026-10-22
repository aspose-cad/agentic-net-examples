using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadCircleExport
{
    class Program
    {
        static void Main(string[] args)
        {
            const string templatePath = "template.dxf";
            const string outputPath = "output.dxf";

            if (!File.Exists(templatePath))
            {
                Console.Error.WriteLine($"Template file \"{templatePath}\" does not exist.");
                return;
            }

            try
            {
                using (CadImage cadImage = (CadImage)Image.Load(templatePath))
                {
                    // Define circles
                    var circle1 = new CadCircle
                    {
                        CenterPoint = new Cad3DPoint(0.0, 0.0, 0.0),
                        Radius = 50.0,
                        LayerName = "CIRCLES"
                    };

                    var circle2 = new CadCircle
                    {
                        CenterPoint = new Cad3DPoint(100.0, 0.0, 0.0),
                        Radius = 30.0,
                        LayerName = "CIRCLES"
                    };

                    var circle3 = new CadCircle
                    {
                        CenterPoint = new Cad3DPoint(0.0, 100.0, 0.0),
                        Radius = 40.0,
                        LayerName = "CIRCLES"
                    };

                    // Add circles to the drawing
                    var newEntities = cadImage.Entities
                        .Concat(new CadEntityBase[] { circle1, circle2, circle3 })
                        .ToArray();

                    cadImage.Entities = newEntities;

                    // Save as DXF
                    var dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };

                    cadImage.Save(outputPath, dxfOptions);
                    Console.WriteLine($"DXF file saved to \"{outputPath}\".");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}