using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class PltToJpegBatchConverter
{
    static void Main(string[] args)
    {
        // Expect two arguments: input folder and output folder
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: PltToJpegBatchConverter <input_folder> <output_folder>");
            return;
        }

        string inputFolder = args[0];
        string outputFolder = args[1];

        // Validate input folder
        if (!Directory.Exists(inputFolder))
        {
            Console.WriteLine($"Input folder does not exist: {inputFolder}");
            return;
        }

        // Ensure output folder exists
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        // Get all PLT files in the input folder (non‑recursive)
        string[] pltFiles = Directory.GetFiles(inputFolder, "*.plt", SearchOption.TopDirectoryOnly);
        if (pltFiles.Length == 0)
        {
            Console.WriteLine("No PLT files found in the specified input folder.");
            return;
        }

        foreach (string pltPath in pltFiles)
        {
            try
            {
                // Load the PLT file as a CadImage
                using (CadImage cadImage = (CadImage)Image.Load(pltPath))
                {
                    // Configure JPEG export options
                    JpegOptions jpegOptions = new JpegOptions
                    {
                        // Maximum JPEG quality
                        Quality = 100,
                        // Rasterization options for CAD rendering
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            // Use millimeters as the unit; automatic layout scaling preserves original size
                            UnitType = UnitType.Millimeter,
                            AutomaticLayoutsScaling = true,
                            // Page dimensions are calculated automatically when set to 0
                            PageWidth = 0,
                            PageHeight = 0,
                            // Optional: line scale can be adjusted for higher detail
                            LineScale = 1.0f
                        }
                    };

                    // Build output file path with .jpg extension
                    string outputFileName = Path.GetFileNameWithoutExtension(pltPath) + ".jpg";
                    string outputPath = Path.Combine(outputFolder, outputFileName);

                    // Save the rendered image as JPEG
                    cadImage.Save(outputPath, jpegOptions);
                    Console.WriteLine($"Converted: {Path.GetFileName(pltPath)} -> {outputFileName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting '{pltPath}': {ex.Message}");
            }
        }
    }
}