using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace AdjustCadDrawingProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.svg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members if needed
                    CadImage cad = (CadImage)cadImage;

                    // Set up rasterization options for SVG output
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        // Note: LineWidthScaleFactor is not available in this SDK version.
                        // Adjustments to line thickness can be handled via other means if required.
                    };

                    // Configure SVG options
                    SvgOptions svgOptions = new SvgOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as SVG
                    cad.Save(outputPath, svgOptions);
                    Console.WriteLine($"SVG file saved successfully to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during loading or saving
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
