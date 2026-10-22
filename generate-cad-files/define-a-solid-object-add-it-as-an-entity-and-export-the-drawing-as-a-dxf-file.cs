using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Define vertices of the solid (a rectangular face)
                var p1 = new Cad3DPoint(0, 0, 0);
                var p2 = new Cad3DPoint(100, 0, 0);
                var p3 = new Cad3DPoint(100, 100, 0);
                var p4 = new Cad3DPoint(0, 100, 0);

                // Create a solid entity (using 3D face)
                var solid = new Cad3DFace(p1, p2, p3, p4);

                // Add the solid entity to the drawing
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { solid }).ToArray();

                // Ensure output directory exists
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "output.dxf");

                // Save the drawing as DXF
                var dxfOptions = new DxfOptions { OutputMode = CadOutputMode.Convert };
                cadImage.Save(outputPath, dxfOptions);

                Console.WriteLine($"DXF file saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}