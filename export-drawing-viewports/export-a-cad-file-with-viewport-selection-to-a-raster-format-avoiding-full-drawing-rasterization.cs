using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Usage: program <inputFile> <outputFile>");
            return;
        }

        string inputPath = args[0];
        string outputPath = args[1];

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine("Input file does not exist: " + inputPath);
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                PngOptions options = new PngOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.UnitType = UnitType.Centimenter;
                rasterOptions.PageWidth = 800;
                rasterOptions.PageHeight = 600;
                rasterOptions.Margins = new Margins() { Left = 0, Right = 0, Top = 0, Bottom = 0 };
                rasterOptions.Layouts = new string[] { "Model" };
                options.VectorRasterizationOptions = rasterOptions;
                image.Save(outputPath, options);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}