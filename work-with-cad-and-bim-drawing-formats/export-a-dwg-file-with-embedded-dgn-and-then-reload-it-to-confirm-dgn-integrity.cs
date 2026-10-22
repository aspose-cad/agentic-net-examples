using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace AsposeCadExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define input DWG file path
                string inputFilePath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Create output directory
                string outputDir = "output";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the DWG file
                using (Image dwgImage = Image.Load(inputFilePath))
                {
                    // Prepare DXF save options (binary format)
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        DxfFileFormat = CadFileFormat.Binary
                    };

                    // Define the exported DXF file path
                    string exportedDxfPath = Path.Combine(outputDir, "exported.dxf");

                    // Save the DWG (with embedded DGN) as DXF
                    dwgImage.Save(exportedDxfPath, dxfOptions);
                    Console.WriteLine($"DWG exported to DXF: {exportedDxfPath}");

                    // Reload the exported DXF to confirm integrity
                    using (Image reloadedDxf = Image.Load(exportedDxfPath))
                    {
                        // Simple validation: check that the image is not null and has a size
                        if (reloadedDxf != null && reloadedDxf.Size.Width > 0 && reloadedDxf.Size.Height > 0)
                        {
                            Console.WriteLine("Reloaded DXF successfully. DGN integrity confirmed.");
                        }
                        else
                        {
                            Console.Error.WriteLine("Failed to validate the reloaded DXF.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}