using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output directories
            string inputDirectory = "InputCAD";
            string outputDirectory = "Output";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Create output directory if it does not exist
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Supported CAD file extensions
            string[] cadExtensions = new string[] { ".dwg", ".dxf", ".dgn", ".dwf", ".ifc", ".stl", ".obj", ".cf2", ".iges", ".step", ".plt" };

            // Get all CAD files in the input directory
            string[] cadFiles = Directory.GetFiles(inputDirectory)
                                         .Where(file => cadExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                                         .ToArray();

            if (cadFiles.Length == 0)
            {
                Console.Error.WriteLine("No CAD files found in the input directory.");
                return;
            }

            foreach (string cadFilePath in cadFiles)
            {
                // Load CAD image
                using (Image cadImage = Image.Load(cadFilePath))
                {
                    string baseFileName = Path.GetFileNameWithoutExtension(cadFilePath);

                    // Convert to PNG
                    string pngOutputPath = Path.Combine(outputDirectory, baseFileName + ".png");
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(pngOutputPath, pngOptions);
                    Console.WriteLine($"Saved PNG: {pngOutputPath}");

                    // Convert to PDF
                    string pdfOutputPath = Path.Combine(outputDirectory, baseFileName + ".pdf");
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(pdfOutputPath, pdfOptions);
                    Console.WriteLine($"Saved PDF: {pdfOutputPath}");
                }
            }

            Console.WriteLine("Batch conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}