using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Input CAD file (DWG/DXF etc.)
        string inputPath = "input.dwg";
        // Output PDF file
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD image
            Image image = Image.Load(inputPath);
            if (image is not CadImage cadImage)
            {
                Console.Error.WriteLine("The provided file is not a supported CAD format.");
                return;
            }

            // Layouts that we want to export – replace with actual layout names present in the file
            string[] layoutsToExport = new[] { "Model" };

            // Prepare rasterization options
            var rasterOptions = new CadRasterizationOptions
            {
                Layouts = layoutsToExport,
                // Example of a custom setting – LineScale influences line thickness; adjust as needed
                LineScale = 1.0f,
                // Unit type can be set if required (e.g., UnitType.Centimeter)
                // UnitType = UnitType.Centimeter
            };

            // Verify that the requested layouts exist in the CAD file
            bool allLayoutsFound = true;
            foreach (string layoutName in layoutsToExport)
            {
                if (!cadImage.Layouts.ContainsKey(layoutName))
                {
                    Console.Error.WriteLine($"Layout '{layoutName}' not found in the CAD file.");
                    allLayoutsFound = false;
                }
            }

            if (!allLayoutsFound)
            {
                Console.Error.WriteLine("One or more specified layouts are missing. Export aborted.");
                return;
            }

            // (Optional) If you need to set page dimensions manually, you can retrieve them from the layout dictionary.
            // Since CadLayout does not expose Width/Height directly, we rely on the layout's default size.

            // Configure PDF export options
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the CAD image as PDF
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"Export completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}