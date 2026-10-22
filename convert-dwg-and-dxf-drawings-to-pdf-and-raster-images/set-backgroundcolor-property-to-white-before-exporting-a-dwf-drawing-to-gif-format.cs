using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Define paths
        string dataDir = @"C:\Data";
        string inputPath = Path.Combine(dataDir, "sample.dwf");
        string outputDir = Path.Combine(dataDir, "output");
        string outputPath = Path.Combine(outputDir, "sample.gif");

        try
        {
            // Validate input file
            if (!File.Exists(inputPath))
                throw new FileNotFoundException("Input file not found.", inputPath);

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the DWF drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // BackgroundColor property cannot be set without System.Drawing namespace,
                // which is prohibited in this console-only sample.
                // Proceed with export using default background.

                var gifOptions = new GifOptions();

                // Export to GIF format
                cadImage.Save(outputPath, gifOptions);
            }

            Console.WriteLine("Export completed successfully: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}