using Aspose.CAD.FileFormats.Cad;
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
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "Output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "output.obj");

            // Load the CAD image
            using (Image loadedImage = Image.Load(inputPath))
            {
                if (loadedImage is CadImage cadImage)
                {
                    // Set OBJ export options (default observer point is at the origin)
                    ObjOptions objOptions = new ObjOptions();

                    // Export to OBJ format
                    cadImage.Save(outputPath, objOptions);
                }
                else
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                }
            }

            Console.WriteLine($"OBJ file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}