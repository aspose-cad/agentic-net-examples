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
        const string inputPath = "input.dwg";
        const string outputDir = "output";

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
                CadImage cadImage = (CadImage)image;

                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    string layoutName = layout.LayoutName;

                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layouts = new[] { layoutName }
                    };

                    SvgOptions svgOptions = new SvgOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string safeLayoutName = string.IsNullOrWhiteSpace(layoutName) ? "default" : layoutName;
                    string outputPath = Path.Combine(outputDir, $"output_{safeLayoutName}.svg");

                    cadImage.Save(outputPath, svgOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}