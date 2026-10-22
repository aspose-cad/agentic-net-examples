using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class CadExportWithRasterValidation
{
    static void Main()
    {
        try
        {
            // Input and output paths
            string inputFilePath = @"C:\CAD\sample.dxf";
            string outputFilePath = @"C:\CAD\Export\sample.png";

            // Verify input file exists
            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException("Input CAD file not found.", inputFilePath);

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load CAD document
            using (CadImage cadImage = (CadImage)Image.Load(inputFilePath))
            {
                // Validate that every external raster image definition points to an existing file
                foreach (CadBaseObject obj in cadImage.Objects)
                {
                    if (obj is CadRasterImageDef rasterDef)
                    {
                        // The property that holds the external file path is FileName
                        string externalPath = rasterDef.FileName;

                        // If the definition references a file, ensure it exists on disk
                        if (!string.IsNullOrEmpty(externalPath) && !File.Exists(externalPath))
                        {
                            throw new FileNotFoundException(
                                $"External raster image referenced by the CAD file was not found: {externalPath}");
                        }
                    }
                }

                // Configure rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    // Example: set unit type; other properties can be set as needed
                    UnitType = UnitType.Inch
                };

                // Set up PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD document to PNG
                cadImage.Save(outputFilePath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}