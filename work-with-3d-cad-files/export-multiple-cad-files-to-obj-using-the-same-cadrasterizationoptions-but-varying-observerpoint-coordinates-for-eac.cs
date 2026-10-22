using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD files (replace with actual paths)
            string[] inputFiles = new string[]
            {
                "model1.dwg",
                "model2.dwg",
                "model3.dwg"
            };

            // Output directory
            string outputDir = "ExportedObj";
            Directory.CreateDirectory(outputDir);

            // Common rasterization options (same for all exports)
            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 8.0f,   // inches
                PageHeight = 6.0f,  // inches
                UnitType = UnitType.Inch,
                Layouts = new string[] { } // use first layout
            };

            // Iterate over each CAD file and export to OBJ
            for (int i = 0; i < inputFiles.Length; i++)
            {
                string inputPath = inputFiles[i];
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    continue;
                }

                // Load CAD image
                using (Image image = Image.Load(inputPath))
                {
                    if (image is not CadImage cadImage)
                    {
                        Console.Error.WriteLine($"Failed to load CAD image: {inputPath}");
                        continue;
                    }

                    // Prepare OBJ export options
                    var objOptions = new ObjOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Build output file path
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(inputPath);
                    string outputPath = Path.Combine(outputDir, $"{fileNameWithoutExt}_view{i + 1}.obj");

                    // Save as OBJ
                    cadImage.Save(outputPath, objOptions);
                    Console.WriteLine($"Exported {inputPath} to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}