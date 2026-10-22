using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadToSvgConverter
{
    class Program
    {
        static void Main()
        {
            // Define input DWG file path (replace with actual path or pass as argument)
            string inputFilePath = @"input\sample.dwg";

            // Define output directory and SVG file name
            string outputDirectory = @"output";
            string outputFileName = "sample_converted.svg";
            string outputFilePath = Path.Combine(outputDirectory, outputFileName);

            // Verify that the source DWG file exists
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Error: The source CAD file \"{inputFilePath}\" was not found.");
                return;
            }

            try
            {
                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the DWG drawing using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Configure rasterization options that control how the CAD data is rendered to SVG
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

                    // Set measurement unit (Centimeter is the correct enum value)
                    rasterOptions.UnitType = UnitType.Centimenter;

                    // Scale of line widths; 1.0 means original width
                    rasterOptions.LineScale = 1.0f;

                    // Define the size of the resulting SVG canvas (in the chosen unit)
                    rasterOptions.PageWidth = 800;   // 800 centimeters – adjust as needed
                    rasterOptions.PageHeight = 600;  // 600 centimeters – adjust as needed

                    // Enable automatic scaling of layouts to fit the page size
                    rasterOptions.AutomaticLayoutsScaling = true;

                    // Set margins around the drawing (values are in the same unit as PageWidth/PageHeight)
                    rasterOptions.Margins = new Margins
                    {
                        Left = 10,
                        Right = 10,
                        Top = 10,
                        Bottom = 10
                    };

                    // Optionally limit conversion to specific layouts; null or empty means all layouts
                    // Example: rasterOptions.Layouts = new string[] { "Model" };
                    rasterOptions.Layouts = null;

                    // Create SVG export options and attach the rasterization settings
                    SvgOptions svgOptions = new SvgOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the image as SVG using the configured options
                    cadImage.Save(outputFilePath, svgOptions);
                }

                Console.WriteLine($"Conversion successful. SVG file saved to \"{outputFilePath}\".");
            }
            catch (Exception ex)
            {
                // Output concise error information
                Console.Error.WriteLine($"An error occurred during conversion: {ex.Message}");
            }
        }
    }
}