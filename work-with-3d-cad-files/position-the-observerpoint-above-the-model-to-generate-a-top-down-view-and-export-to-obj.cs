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
            // Define input and output paths
            string inputPath = "input.dwg";
            string outputDir = "Output";
            string outputPath = Path.Combine(outputDir, "top_down_view.obj");

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            Directory.CreateDirectory(outputDir);

            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set up OBJ export options (default view)
                var objOptions = new ObjOptions();

                // Save the model to OBJ format
                cadImage.Save(outputPath, objOptions);
                Console.WriteLine($"OBJ file saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}