using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "output.dxf");

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        Directory.CreateDirectory(outputDir);

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("The loaded file is not a supported CAD image.");
                    return;
                }

                // Set default font for text entities
                cadImage.DefaultFont = "Arial";

                // Iterate over entities to handle individual text objects if needed
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadText)
                    {
                        // Font is controlled by DefaultFont; additional per-entity settings can be added here
                    }
                    else if (entity is CadMText)
                    {
                        // Same as above for MText
                    }
                }

                // Simple layout arrangement: set default extents for each layout
                foreach (var layout in cadImage.Layouts.Values)
                {
                    layout.MinExtents = new Cad3DPoint(0, 0, 0);
                    layout.MaxExtents = new Cad3DPoint(100, 100, 0);
                }

                // Save as DXF preserving editable objects
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                cadImage.Save(outputPath, dxfOptions);
            }

            Console.WriteLine($"DXF file saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}