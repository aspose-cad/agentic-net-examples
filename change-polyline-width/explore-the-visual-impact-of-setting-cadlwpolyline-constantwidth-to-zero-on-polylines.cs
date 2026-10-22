using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                CadLwPolyline targetPolyline = null;
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadLwPolyline lwPolyline)
                    {
                        targetPolyline = lwPolyline;
                        break;
                    }
                }

                if (targetPolyline == null)
                {
                    Console.Error.WriteLine("No CadLwPolyline entity found in the drawing.");
                }
                else
                {
                    // Set ConstantWidth to zero to observe visual impact
                    targetPolyline.ConstantWidth = 0.0;
                }

                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, saveOptions);
                Console.WriteLine($"Modified drawing saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}