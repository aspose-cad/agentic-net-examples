using Aspose.CAD.FileFormats.Cad.CadTables;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class ApplyDefaultFont
{
    static void Main(string[] args)
    {
        // Input CAD file path (replace with actual path)
        string inputPath = @"C:\CAD\Input\sample.dxf";

        // Output directory and file
        string outputDir = @"C:\CAD\Output";
        string outputPath = Path.Combine(outputDir, "sample_modified.dxf");

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }
        }

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Set a new default font for the drawing
                string newDefaultFont = "Times New Roman";
                cadImage.DefaultFont = newDefaultFont;
                Console.WriteLine($"Default font set to \"{newDefaultFont}\".");

                // Inspect top‑level entities
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadText cadText)
                    {
                        Console.WriteLine($"CadText entity at ({cadText.FirstAlignment.X}, {cadText.FirstAlignment.Y}, {cadText.FirstAlignment.Z})");
                    }
                    else if (entity is CadMText cadMText)
                    {
                        Console.WriteLine($"CadMText entity at ({cadMText.InsertionPoint.X}, {cadMText.InsertionPoint.Y}, {cadMText.InsertionPoint.Z})");
                    }
                }

                // Inspect block definitions and their entities to confirm inheritance
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    string blockHandle = block.CadBlockTableObject?.ObjectHandle ?? "Unnamed";
                    Console.WriteLine($"Inspecting block: {blockHandle}");

                    foreach (var blockEntity in block.Entities)
                    {
                        if (blockEntity is CadText blockText)
                        {
                            Console.WriteLine($"  Block CadText at ({blockText.FirstAlignment.X}, {blockText.FirstAlignment.Y}, {blockText.FirstAlignment.Z}) inherits default font \"{cadImage.DefaultFont}\".");
                        }
                        else if (blockEntity is CadMText blockMText)
                        {
                            Console.WriteLine($"  Block CadMText at ({blockMText.InsertionPoint.X}, {blockMText.InsertionPoint.Y}, {blockMText.InsertionPoint.Z}) inherits default font \"{cadImage.DefaultFont}\".");
                        }
                    }
                }

                // Save the modified CAD drawing
                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                cadImage.Save(outputPath);
                Console.WriteLine($"Modified CAD file saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}