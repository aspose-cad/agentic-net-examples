using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
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
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Iterate over top‑level entities and modify lines
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine line)
                    {
                        // Shift the line 10 units along the X axis
                        Cad3DPoint first = line.FirstPoint;
                        Cad3DPoint second = line.SecondPoint;

                        line.FirstPoint = new Cad3DPoint(first.X + 10, first.Y, first.Z);
                        line.SecondPoint = new Cad3DPoint(second.X + 10, second.Y, second.Z);
                    }
                }

                // Attempt to save the modified drawing to DWG format.
                // Aspose.CAD does not provide a direct DWG save option; the closest supported CAD format is DXF.
                // Therefore, we save as DXF using DxfOptions. The file extension is kept as .dwg for compatibility,
                // but the actual format will be DXF. Adjust as needed based on SDK capabilities.
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                cadImage.Save(outputPath, dxfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}