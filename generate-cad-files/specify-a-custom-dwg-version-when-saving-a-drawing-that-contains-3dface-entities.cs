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
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to work with CAD entities
                CadImage cadImage = (CadImage)image;

                // Define four 3D points for the 3DFACE entity
                Cad3DPoint p1 = new Cad3DPoint(0, 0, 0);
                Cad3DPoint p2 = new Cad3DPoint(100, 0, 0);
                Cad3DPoint p3 = new Cad3DPoint(100, 100, 0);
                Cad3DPoint p4 = new Cad3DPoint(0, 100, 0);

                // Create the 3DFACE entity
                Cad3DFace threeDFace = new Cad3DFace(p1, p2, p3, p4);

                // Append the new entity to the drawing
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { threeDFace }).ToArray();

                // Prepare DWG save options (custom version not exposed in current API)
                DwgOptions dwgOptions = new DwgOptions
                {
                    // Example: set output mode if needed
                    OutputMode = CadOutputMode.Convert
                };

                // Save the modified drawing
                cadImage.Save(outputPath, dwgOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}