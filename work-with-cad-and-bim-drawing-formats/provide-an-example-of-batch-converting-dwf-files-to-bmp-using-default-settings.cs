using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace BatchDwfToBmp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output directories (adjust paths as needed)
                string inputFolder = "InputDwf";
                string outputFolder = "OutputBmp";

                // Verify input directory exists
                if (!Directory.Exists(inputFolder))
                {
                    Console.Error.WriteLine($"Input directory does not exist: {inputFolder}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // Get all DWF files in the input directory
                string[] dwfFiles = Directory.GetFiles(inputFolder, "*.dwf", SearchOption.TopDirectoryOnly);
                if (dwfFiles.Length == 0)
                {
                    Console.WriteLine("No DWF files found to convert.");
                    return;
                }

                // Process each DWF file
                foreach (string dwfFilePath in dwfFiles)
                {
                    try
                    {
                        // Load the DWF file
                        using (Image cadImage = Image.Load(dwfFilePath))
                        {
                            // Prepare BMP save options (default settings)
                            BmpOptions bmpOptions = new BmpOptions();

                            // Build output file path with .bmp extension
                            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(dwfFilePath);
                            string bmpFilePath = Path.Combine(outputFolder, fileNameWithoutExt + ".bmp");

                            // Save the image as BMP
                            cadImage.Save(bmpFilePath, bmpOptions);
                            Console.WriteLine($"Converted: {dwfFilePath} -> {bmpFilePath}");
                        }
                    }
                    catch (Exception exFile)
                    {
                        Console.Error.WriteLine($"Error converting file '{dwfFilePath}': {exFile.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}