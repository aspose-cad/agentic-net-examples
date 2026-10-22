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
            // Define input CAD file and output directory
            string inputPath = "input.dwg";
            string outputDirectory = "output";
            string outputPath = Path.Combine(outputDirectory, "model.obj");

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure OBJ export options
                ObjOptions objOptions = new ObjOptions();
                // The OBJ exporter generates an accompanying .mtl file by default.

                // Save the CAD model to OBJ format
                cadImage.Save(outputPath, objOptions);
            }

            Console.WriteLine($"OBJ file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}