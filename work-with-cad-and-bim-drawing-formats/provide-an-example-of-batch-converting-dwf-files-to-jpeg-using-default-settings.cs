using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output directories (replace with actual paths)
            string inputDirectory = @"C:\InputDwfFiles";
            string outputDirectory = @"C:\OutputJpegFiles";

            // Verify that the input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Create the output directory if it does not exist
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Get all DWF files in the input directory
            string[] dwfFiles = Directory.GetFiles(inputDirectory, "*.dwf", SearchOption.TopDirectoryOnly);

            foreach (string dwfFilePath in dwfFiles)
            {
                // Load the DWF file
                using (Image dwfImage = Image.Load(dwfFilePath))
                {
                    // Prepare output JPEG file path
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(dwfFilePath);
                    string jpegFilePath = Path.Combine(outputDirectory, fileNameWithoutExtension + ".jpg");

                    // Save as JPEG using default settings
                    JpegOptions jpegOptions = new JpegOptions();
                    dwfImage.Save(jpegFilePath, jpegOptions);

                    Console.WriteLine($"Converted: {dwfFilePath} -> {jpegFilePath}");
                }
            }

            Console.WriteLine("Batch conversion completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}