using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file path
            string inputPath = "input.dwg";

            // Custom font folder path
            string customFontFolder = @"C:\CustomFonts";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure custom font folder (DefaultFont property expects a font name or path)
                cadImage.DefaultFont = customFontFolder;

                // Example operation: list entity type names
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    Console.WriteLine($"Entity type: {entity.TypeName}");
                }

                // Save the CAD drawing preserving editable objects (DXF format as example)
                string outputPath = "output.dxf";
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, dxfOptions);
                Console.WriteLine($"CAD file saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}