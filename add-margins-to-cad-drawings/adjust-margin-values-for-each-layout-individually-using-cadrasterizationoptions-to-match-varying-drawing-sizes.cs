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
            // Input and output file paths (replace with actual paths as needed)
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options for the first layout
            CadRasterizationOptions rasterOptionsModel = new CadRasterizationOptions
            {
                Layouts = new[] { "Model" }
                // If margin handling is required, adjust other properties such as PageSize or Scale here.
            };

            // Configure rasterization options for the second layout
            CadRasterizationOptions rasterOptionsLayout1 = new CadRasterizationOptions
            {
                Layouts = new[] { "Layout1" }
                // Adjust properties per layout as needed.
            };

            // Example: render the Model layout
            PngOptions pngOptionsModel = new PngOptions
            {
                VectorRasterizationOptions = rasterOptionsModel
            };
            cadImage.Save("model_" + outputPath, pngOptionsModel);

            // Example: render the Layout1 layout
            PngOptions pngOptionsLayout1 = new PngOptions
            {
                VectorRasterizationOptions = rasterOptionsLayout1
            };
            cadImage.Save("layout1_" + outputPath, pngOptionsLayout1);

            // Dispose the image when done
            cadImage.Dispose();

            Console.WriteLine($"CAD drawing saved with separate layout images.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
