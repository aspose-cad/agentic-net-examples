using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.obj";
            string outputPath = "output.pdf";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            // Prepare load options
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            try
            {
                using (Image image = Image.Load(inputPath, loadOptions))
                {
                    Image cadImage = image ;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Loaded image is not a CAD image.");
                        return;
                    }

                    var rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Millimeter,
                        PageWidth = 210,
                        PageHeight = 297,
                        AutomaticLayoutsScaling = true
                    };

                    // Callback to capture rendering errors
                    // Note: The CadRenderResult type does not expose detailed error properties in the current SDK.
                    // Therefore we can only acknowledge that the callback was invoked.
                    rasterOptions.RenderResult = renderResult =>
                    {
                        Console.WriteLine("Render callback invoked.");
                    };

                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"PDF saved to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing the OBJ file: " + ex.Message);
                foreach (var loadError in loadOptions.Errors)
                {
                    Console.WriteLine($"Load error: {loadError.Message}, Code: {loadError.RenderCode}");
                }
            }
        }
    }
}