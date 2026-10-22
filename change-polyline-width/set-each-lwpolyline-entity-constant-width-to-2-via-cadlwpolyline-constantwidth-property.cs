using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Set constant width of each LWPOLYLINE entity to 2
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    CadLwPolyline lwPolyline = entity as CadLwPolyline;
                    if (lwPolyline != null)
                    {
                        lwPolyline.ConstantWidth = 2.0;
                    }
                }

                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                cadImage.Save(outputPath, dxfOptions);
                Console.WriteLine($"File saved successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}