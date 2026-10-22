using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class RemoveBordersFromCanvas
{
    static void Main(string[] args)
    {
        // Expect input CAD file path and output PNG file path as arguments
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: RemoveBordersFromCanvas <input-cad-file> <output-png-file>");
            return;
        }

        string inputFile = args[0];
        string outputFile = args[1];

        // Validate input file existence
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file does not exist: {inputFile}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputFile);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load CAD image
            using (Aspose.CAD.FileFormats.Cad.CadImage cadImage = (Aspose.CAD.FileFormats.Cad.CadImage)Image.Load(inputFile))
            {
                // Retrieve and display layer information
                Console.WriteLine("Layers in the drawing:");
                foreach (Aspose.CAD.FileFormats.Cad.CadTables.CadLayerTable layer in cadImage.Layers)
                {
                    Console.WriteLine($"- Name: {layer.Name}, ColorId: {layer.ColorId}, Flags: {layer.Flags}, PlotFlag: {layer.PlotFlag}");
                }

                // Configure rasterization options with Zoom = 1 and zero margins for clean edges
                var rasterOptions = new Aspose.CAD.ImageOptions.CadRasterizationOptions
                {
                    Zoom = 1,
                    UnitType = Aspose.CAD.ImageOptions.UnitType.Centimenter,
                    Margins = new Aspose.CAD.ImageOptions.Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                };

                // Set up PNG export options
                var pngOptions = new Aspose.CAD.ImageOptions.PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the drawing
                cadImage.Save(outputFile, pngOptions);
                Console.WriteLine($"Export completed: {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}