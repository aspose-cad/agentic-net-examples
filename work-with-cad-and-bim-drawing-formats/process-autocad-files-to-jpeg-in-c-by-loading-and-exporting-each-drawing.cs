using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadToJpegExport
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Define input and output directories (replace with actual paths)
                    string inputFolder = @"C:\CadFiles\Input";
                    string outputFolder = @"C:\CadFiles\Output";

                    // Ensure the output directory exists
                    if (!Directory.Exists(outputFolder))
                    {
                        Directory.CreateDirectory(outputFolder);
                    }

                    // Process all DWG files in the input folder
                    string[] cadFiles = Directory.GetFiles(inputFolder, "*.dwg", SearchOption.TopDirectoryOnly);
                    foreach (string cadFilePath in cadFiles)
                    {
                        // Load the CAD drawing
                        using (Image cadImage = Image.Load(cadFilePath))
                        {
                            // Prepare JPEG export options
                            JpegOptions jpegOptions = new JpegOptions();

                            // Build the output file path
                            string outputFileName = Path.GetFileNameWithoutExtension(cadFilePath) + ".jpg";
                            string outputFilePath = Path.Combine(outputFolder, outputFileName);

                            // Save the drawing as JPEG
                            cadImage.Save(outputFilePath, jpegOptions);
                        }
                    }

                    Console.WriteLine("Export completed.");
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}