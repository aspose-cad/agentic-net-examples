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
            // Define input and output directories
            string inputFolder = "InputPltFiles";
            string outputFolder = "OutputJpegFiles";

            // Verify input directory exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Create output directory if it does not exist
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Get all PLT files in the input directory
            string[] pltFiles = Directory.GetFiles(inputFolder, "*.plt", SearchOption.TopDirectoryOnly);
            if (pltFiles.Length == 0)
            {
                Console.WriteLine("No PLT files found to convert.");
                return;
            }

            // Configure JPEG save options
            JpegOptions jpegOptions = new JpegOptions();
            jpegOptions.Quality = 90; // Set desired JPEG quality (0-100)

            // Process each PLT file
            foreach (string pltFilePath in pltFiles)
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(pltFilePath);
                string jpegFilePath = Path.Combine(outputFolder, fileNameWithoutExt + ".jpg");

                // Load the PLT file
                using (Image cadImage = Image.Load(pltFilePath))
                {
                    // Save as JPEG
                    cadImage.Save(jpegFilePath, jpegOptions);
                }

                Console.WriteLine($"Converted {pltFilePath} to {jpegFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}