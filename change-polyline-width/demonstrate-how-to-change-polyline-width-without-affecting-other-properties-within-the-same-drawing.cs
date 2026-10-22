using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

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
                CadImage cadImage = (CadImage)image;

                // Define the new line weight (width) for polylines (in hundredths of mm)
                short newLineWeight = 200; // example: 2 mm

                // Process 2D lightweight polylines
                foreach (CadLwPolyline lwPolyline in cadImage.Entities.OfType<CadLwPolyline>())
                {
                    lwPolyline.LineWeight = newLineWeight;
                }

                // Process 3D polylines
                foreach (CadPolyline polyline3D in cadImage.Entities.OfType<CadPolyline>())
                {
                    polyline3D.LineWeight = newLineWeight;
                }

                // Save the modified drawing preserving editable objects
                DwgOptions dwgOptions = new DwgOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, dwgOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}