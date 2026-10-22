using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputDirPath = "Input";
            string outputDirPath = "Output";

            if (!Directory.Exists(inputDirPath))
            {
                Console.Error.WriteLine($"Input directory '{inputDirPath}' does not exist.");
                return;
            }

            if (!Directory.Exists(outputDirPath))
            {
                Directory.CreateDirectory(outputDirPath);
            }

            string[] dwfFiles = Directory.GetFiles(inputDirPath, "*.dwf", SearchOption.TopDirectoryOnly);
            if (dwfFiles.Length == 0)
            {
                Console.WriteLine("No DWF files found to convert.");
                return;
            }

            foreach (string dwfFilePath in dwfFiles)
            {
                // Load DWF file
                using (Image dwfImage = Image.Load(dwfFilePath))
                {
                    // Prepare BMP save options
                    BmpOptions bmpOptions = new BmpOptions();

                    // Determine output file path
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(dwfFilePath);
                    string bmpFilePath = Path.Combine(outputDirPath, fileNameWithoutExt + ".bmp");

                    // Save as BMP
                    dwfImage.Save(bmpFilePath, bmpOptions);
                    Console.WriteLine($"Converted '{dwfFilePath}' to '{bmpFilePath}'.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}