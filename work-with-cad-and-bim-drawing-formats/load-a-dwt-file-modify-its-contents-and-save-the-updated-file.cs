using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CadDwtModifyExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths (replace with actual paths)
            string inputPath = "input.dwt";
            string outputPath = "output.dwt";

            try
            {
                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the DWT file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage for CAD-specific operations
                    CadImage cadImage = (CadImage)image;

                    // Example modification: change the color of the first line entity to Red (ColorId = 1)
                    foreach (var entity in cadImage.Entities)
                    {
                        // Check if the entity is a line
                        if (entity.TypeName != null &&
                            entity.TypeName.ToString().Equals("Line", StringComparison.OrdinalIgnoreCase))
                        {
                            // Set indexed color to red (ColorId = 1)
                            entity.ColorId = 1;
                            break;
                        }
                    }

                    // Save the modified DWT file
                    cadImage.Save(outputPath, null);
                }

                Console.WriteLine($"File saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}