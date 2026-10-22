using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadTextHeightAdjustment
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file (DWG or DXF)
                string sourcePath = "input.dwg";
                // Path for the modified CAD file
                string outputPath = "output.dwg";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Desired uniform text height
                double targetTextHeight = 2.5;

                // Load the CAD drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate over top‑level entities in the drawing
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Adjust height for single‑line text entities
                        if (entity is CadText cadText)
                        {
                            cadText.TextHeight = targetTextHeight;
                        }
                        // Adjust height for multi‑line text entities
                        else if (entity is CadMText cadMText)
                        {
                            cadMText.InitialTextHeight = targetTextHeight;
                        }
                    }

                    // Save the modified drawing back to DWG format
                    cadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}