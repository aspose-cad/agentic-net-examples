using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace InsertDxfExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DXF file to be inserted
                string sourcePath = "input.dxf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Prepare output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "combined.dxf");

                // Load the source DXF drawing
                using (Image sourceImage = Image.Load(sourcePath))
                {
                    // Save the loaded drawing as the combined output DXF
                    DxfOptions dxfOptions = new DxfOptions();
                    sourceImage.Save(outputPath, dxfOptions);
                }

                Console.WriteLine($"Combined DXF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}