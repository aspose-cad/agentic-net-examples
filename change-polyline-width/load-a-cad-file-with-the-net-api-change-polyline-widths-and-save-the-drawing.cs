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
        try
        {
            // Paths to the source CAD file and the output file
            string inputPath = "input.dwg";
            string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast the loaded image to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Change width for lightweight polylines
                foreach (CadLwPolyline lwPolyline in cadImage.Entities.OfType<CadLwPolyline>())
                {
                    // The Width property is not available; use Thickness to represent line width
                    lwPolyline.Thickness = 0.5;
                }

                // Change width for 3D polylines
                foreach (CadPolyline polyline3D in cadImage.Entities.OfType<CadPolyline>())
                {
                    polyline3D.Thickness = 0.5;
                }

                // Prepare DXF save options that preserve editable objects
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                // Save the modified drawing
                cadImage.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}