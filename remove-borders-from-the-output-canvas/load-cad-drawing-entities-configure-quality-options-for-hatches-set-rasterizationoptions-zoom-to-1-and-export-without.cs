using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

// Input and output paths
string inputPath = @"C:\CAD\sample.dwg";
string outputPath = @"C:\CAD\Export\sample.svg";

try
{
    // Verify that the input file exists
    if (!File.Exists(inputPath))
        throw new FileNotFoundException($"Input file not found: {inputPath}");

    // Ensure the output directory exists
    string? outputDir = Path.GetDirectoryName(outputPath);
    if (string.IsNullOrEmpty(outputDir))
        throw new InvalidOperationException("Unable to determine output directory.");

    Directory.CreateDirectory(outputDir);

    // Load the CAD drawing
    using (CadImage cadImage = (CadImage)Image.Load(inputPath))
    {
        // Configure rasterization options
        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
        {
            // Unit type (spelling as defined in the SDK)
            UnitType = UnitType.Centimenter,
            // Scale of lines; higher values increase line thickness in the rasterized output
            LineScale = 1.0f,
            // Zoom factor – set to 1 as requested
            Zoom = 1.0f,
            // Remove all margins from the exported image
            Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
        };

        // NOTE: The Aspose.CAD API does not expose a HatchQuality property.
        // Hatch rendering quality is controlled internally; no explicit setting is available.

        // Prepare SVG export options and assign the rasterization settings
        SvgOptions svgOptions = new SvgOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Export the CAD drawing to SVG without margins
        cadImage.Save(outputPath, svgOptions);
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine($"An error occurred: {ex.Message}");
    // Optionally rethrow or handle the exception as needed
}