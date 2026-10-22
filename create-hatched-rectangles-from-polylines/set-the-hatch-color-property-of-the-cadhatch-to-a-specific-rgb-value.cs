using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Hatch;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        try
        {
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to prepare output directory: {ex.Message}");
            return;
        }

        CadImage cadImage = null;
        try
        {
            cadImage = (CadImage)Image.Load(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD image: {ex.Message}");
            return;
        }

        try
        {
            // Locate the first hatch entity in the drawing
            CadHatch hatch = cadImage.Entities.OfType<CadHatch>().FirstOrDefault();

            if (hatch != null)
            {
                // Set the hatch's true color using an RGB integer (e.g., 0x00FF00 for green)
                hatch.ColorValue = 0x00FF00;
                Console.WriteLine("Hatch color set to green (0x00FF00).");
            }
            else
            {
                Console.WriteLine("No hatch entity found in the drawing.");
            }

            // Save the modified CAD file
            var dxfOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Binary
            };
            cadImage.Save(outputPath, dxfOptions);
            Console.WriteLine($"Modified CAD saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }
}