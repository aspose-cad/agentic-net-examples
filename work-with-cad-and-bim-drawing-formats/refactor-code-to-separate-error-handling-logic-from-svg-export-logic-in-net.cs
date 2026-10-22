using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadSvgExport
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputPath = "output.svg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                ExportToSvg(inputPath, outputPath);
                Console.WriteLine("SVG exported successfully.");
            }
            catch (Exception ex)
            {
                HandleExportError(ex);
            }
        }

        static void ExportToSvg(string inputFile, string outputFile)
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputFile))
            {
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    PageWidth = 800,
                    PageHeight = 600,
                    AutomaticLayoutsScaling = true,
                    Margins = new Margins
                    {
                        Left = 10,
                        Right = 10,
                        Top = 10,
                        Bottom = 10
                    }
                };

                SvgOptions svgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputFile, svgOptions);
            }
        }

        static void HandleExportError(Exception ex)
        {
            Console.Error.WriteLine($"Error exporting SVG: {ex.Message}");
            // Additional error handling or logging can be added here.
        }
    }
}